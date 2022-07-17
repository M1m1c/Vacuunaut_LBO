using Team14;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "SuctionAction", menuName = "Equipment/Actions/SuctionAction")]
public class SuctionAction : AirFlowAction
{
    [Range(1f, 100f)]
    public float SuctionStrength = 50f;

    [Range(0f, 3f)]
    public float DestructionRange = 1.5f;

    public Vector3 OriginOffset = new Vector3();

    public EDirection Direction;

    public bool ShouldShrink = false;

    private CharacterMovement movement;

    public override void Initalise(Equipment owner)
    {
        if (!owner.transform.parent) { return; }
        if (!owner.transform.parent.CompareTag("MainCamera")) { return; }

        movement = owner.GetComponentInParent<CharacterMovement>();
        if (!movement) { Debug.Log("Could not find CharacterMovement in Parent @Initalise"); }

    }

    public override void DoAction(Equipment owner)
    {
        if (!owner) { return; }

        var cleaner = owner.GetComponent<CleaningEquipment>();
        if (!cleaner) { return; }

        var cleaningVolume = cleaner.CleaningVolume;
        if (!cleaningVolume) { return; }

        var overlappingObjects = cleaningVolume.overlappingObjects;
        if (overlappingObjects.Count <= 0) { return; }

        var transform = cleaner.CleaningOrigin;
        if (!transform) { return; }

        var origin = transform.TransformPoint(OriginOffset);


        for (int i = overlappingObjects.Count - 1; i >= 0; i--)
        {
            var obj = overlappingObjects[i];
            if (!obj)
            {
                overlappingObjects.RemoveAt(i);
                continue;
            }

            var heldObj = cleaner.AttatchedObject;
            if (heldObj != null && heldObj != obj.gameObject) { continue; }

            AddForceInDirection(origin, owner, obj);

            if (ShouldShrink)
            {
                var startSize = obj.StartSize;

                var t = Time.deltaTime * startSize.magnitude;
                var reduction = new Vector3(t, t, t);

                var minShrink = new Vector3(
                    startSize.x * 0.5f,
                    startSize.y * 0.5f,
                    startSize.z * 0.5f);

                obj.transform.localScale = Vector3.Max(
                                obj.transform.localScale - reduction,
                                minShrink);
            }

            var info = new ConditionInfo(
                  Direction,
                  origin,
                  obj.transform.position,
                  owner.transform.position,
                  DestructionRange,
                  cleaner.MaxCapacity,
                  cleaner.CurrentCapacity,
                  obj.RecycleValue);

            CheckDoClean(owner, info, obj);
        }
    }

    private void AddForceInDirection(Vector3 origin, Equipment equipment, CleanableBase obj)
    {
        var objPos = obj.transform.position;
        var outDir = objPos - origin;
        var inDir = origin - objPos;
        var flowDir = (Direction == 0 ? inDir : outDir).normalized;

        var dot = Vector3.Dot((objPos - equipment.transform.position).normalized, flowDir.normalized);
        var dirFacingPlayer = dot < 0f;
        if ((int)Direction == 1 && dirFacingPlayer)
        { flowDir = -flowDir; }

        var rigidbody = obj.GetComponent<Rigidbody>();

        if (obj.Attatched && Direction== EDirection.airIn)
        {
            float mouseDelta = 0f;        
            
            if (Mouse.current != null) 
            {
                var delta = Mouse.current.delta;
                if (delta != null) 
                {
                    mouseDelta = Mathf.Clamp( delta.ReadValue().magnitude *5f,0.01f,80f);
                }
            }

            var moveVelocity = 0f;
            if (movement) { moveVelocity = Mathf.Clamp(movement.Velocity, 0.01f ,100f); }

            rigidbody.velocity = flowDir * Mathf.Clamp( (mouseDelta + moveVelocity + SuctionStrength * Time.deltaTime), 0f, 13f);
            rigidbody.angularVelocity = (rigidbody.angularVelocity * 0.95f);
        }
        else
        {
            rigidbody.AddForce(flowDir * (rigidbody.mass * SuctionStrength));
        }

    }

}

[System.Serializable]
public enum EDirection
{
    airIn,
    airOut
}
