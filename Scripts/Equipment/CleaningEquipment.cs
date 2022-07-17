using UnityEngine;
using UnityEngine.Events;

public class CleaningEquipment : Equipment
{
    public CleaningVolume CleaningVolume;
    public Transform CleaningOrigin;

    public UnityEvent<float> UpdateCapUIEvent = new UnityEvent<float>();

    public GameObject AttatchedObject { get; private set; }

    private float attachedMass = 0.2f;

    public int MaxCapacity { get; private set; }
    public int CurrentCapacity 
    { 
        get { return currentCapacity; } 
        set 
        { 
            currentCapacity = value;
            UpdateCapUIEvent.Invoke((float)currentCapacity / (float)MaxCapacity);
        } 
    }
    private int currentCapacity;

    public float MaxTimeBetweenShots { get; private set; }
    public float DeltaTimeBetweenShots { get; set; }

    private EquipmentAudioManager equipmentAudioManager;

    public override void Setup()
    {
        base.Setup();
        MaxCapacity = Stats.MaxCapacity;
        CurrentCapacity = MaxCapacity/3;
        MaxTimeBetweenShots = Stats.TimeBetweenShots;
        equipmentAudioManager = GetComponent<EquipmentAudioManager>();
    }

    public override void DoPrimaryAction()
    {
        if (primaryAction == null) { return; }
        primaryAction.DoAction(this);
        SetVFX(true, -airVFXSpeed, 0);

        if (Extension && !AttatchedObject) 
        {
            Extension.DoPrimaryAction();
        }      

        if (!equipmentAudioManager) { return; }
        equipmentAudioManager.PlayActiveSound(primaryAction);
    }

    public override void DoSecondaryAction()
    {
        if (AttatchedObject) { DetatchFromOrigin(AttatchedObject); }
        base.DoSecondaryAction();
        if (!equipmentAudioManager) { return; }
        equipmentAudioManager.PlayActiveSound(secondaryAction);
    }

    public override void StopPrimaryAction()
    {
        base.StopPrimaryAction();
        if (!equipmentAudioManager) { return; }
        equipmentAudioManager.PlayDeactiveSound(primaryAction);

        var cleanables = CleaningVolume.overlappingObjects;
        for (int i = 0; i < cleanables.Count; i++)
        {
            var item = cleanables[i];
            if (!item) { continue; }
            item.ResetSize();
        }

        if (!AttatchedObject) { return; }
        DetatchFromOrigin(AttatchedObject);
    }

    public override void StopSecondaryAction()
    {
        base.StopSecondaryAction();
        if (!equipmentAudioManager) { return; }
        equipmentAudioManager.PlayDeactiveSound(secondaryAction);
    }

    public override bool ShouldBypassPrimaryHold()
    {
        return AttatchedObject != null;
    }

    private void Awake()
    {
        CleaningVolume.OnAttatchToOrigin.AddListener(AttatchToOrigin);
        CleaningVolume.OnDetatchFromOrigin.AddListener(DetatchFromOrigin);
    }

    private void FixedUpdate()
    {
        if (DeltaTimeBetweenShots > 0)
        {
            DeltaTimeBetweenShots = Mathf.Clamp(DeltaTimeBetweenShots - Time.fixedDeltaTime, 0f, MaxTimeBetweenShots);
        }

        if (AttatchedObject && CleaningOrigin)
        {
            var dist = Vector3.Distance(AttatchedObject.transform.position, CleaningOrigin.position);
            if (dist > 2.5f)
            {
                var cleanable = AttatchedObject.GetComponent<CleanableBase>();
                if (!cleanable) { return; }
                DetatchFromOrigin(AttatchedObject);
                CleaningVolume.RemoveFromVolume(cleanable);
            }
        }
    }

    private void AttatchToOrigin(GameObject gameObject)
    {
        AttatchedObject = gameObject;
        ToggleMass(gameObject, true);
    }

    private void DetatchFromOrigin(GameObject gameObject)
    {
        AttatchedObject = null;

        ToggleMass(gameObject, false);

        var cleanable = gameObject.GetComponent<CleanableBase>();
        if (!cleanable) { return; }
        cleanable.Attatched = false;
        CleaningVolume.RemoveFromVolume(cleanable);
    }


    private void ToggleKinematic(GameObject gameObject, bool b)
    {
        var rb = gameObject.GetComponent<Rigidbody>();
        if (!rb) { return; }
        rb.isKinematic = b;
    }

    private void ToggleMass(GameObject gameObject, bool b)
    {
        if (!gameObject) { return; }

        var rb = gameObject.GetComponent<Rigidbody>();
        if (!rb) { return; }

        var cleanble = gameObject.GetComponent<CleanableBase>();
        if (!cleanble) { return; }

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (b) 
        { 
            rb.mass = attachedMass;
            rb.useGravity = false;
        }
        else 
        {          
            rb.mass = cleanble.OriginalMass;
            rb.useGravity = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (!CleaningOrigin) { return; }

        Gizmos.color = Color.green;
        Gizmos.DrawCube(CleaningOrigin.position, new Vector3(0.3f, 0.3f, 0.3f));
    }
}
