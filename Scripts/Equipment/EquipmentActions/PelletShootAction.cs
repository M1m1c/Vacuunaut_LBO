using UnityEngine;

[CreateAssetMenu(fileName = "PelletShootAction", menuName = "Equipment/Actions/PelletShootAction")]
public class PelletShootAction : EquipmentAction
{
    public CleanableBase PelletPrefab;

    [Range(500f, 5000f)]
    public float ShootForce = 500f;

    public int ShootCost = 1;

    public AudioClip[] ShootSounds;
    public AudioClip EmptySound;
    public AudioManager AudioManager;

    public override void DoAction(Equipment owner)
    {
        if (!owner) { return; }

        if (!PelletPrefab) { return; }

        var cleaner = owner.GetComponent<CleaningEquipment>();
        if (!cleaner) { return; }

        if (cleaner.DeltaTimeBetweenShots > 0) { return; }
        cleaner.DeltaTimeBetweenShots = cleaner.MaxTimeBetweenShots;

        if (cleaner.CurrentCapacity <= 0) {
            if (EmptySound) { AudioManager.PlayAudio(EmptySound, AudioType.SFX, owner.transform.position); }
            return; 
        }
        cleaner.CurrentCapacity -= ShootCost;

        var targetPos = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.57f, 0)).GetPoint(30f);

        var startPos = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.0f, 0)).GetPoint(0.5f);

        var direction = (targetPos - startPos).normalized;

        var pellet = Instantiate(PelletPrefab, startPos, Quaternion.identity);

        var pelletMesh = pellet.GetComponent<MeshRenderer>();
        if (pelletMesh) { pelletMesh.enabled = false; }

        var pelletRB = pellet.GetComponent<Rigidbody>();
        if (!pelletRB) { return; }

        pelletRB.AddForce(direction * (pelletRB.mass * ShootForce));

        if (ShootSounds.Length <= 0) { return; }
        if (!AudioManager) { return; }

        var rand = Random.Range(0, ShootSounds.Length);
        if (rand < 0 || rand > ShootSounds.Length) { return; }

        var randSound = ShootSounds[rand];
        if (!randSound) { return; }
        AudioManager.PlayAudio(randSound,AudioType.SFX, pellet.transform.position);
    }
}
