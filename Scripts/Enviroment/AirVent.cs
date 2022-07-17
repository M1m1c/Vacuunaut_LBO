using System;
using UnityEngine;

public class AirVent : MonoBehaviour
{
    public AirVent ConnectedAirVent;
    public CleaningEquipment VacuumExtensionPrefab;

    public Vector3 ExtensionOffest = new Vector3(0f, 0f, -1f);
    public Vector3 ExtensionSize = new Vector3(0.5f, 0.5f, 0.5f);

    public float MinDistance = 4f;

    public CleanableBase ignore { get; set; }

    private CleaningEquipment vacuumExtension;

    private void OnTriggerEnter(Collider other)
    {
        if (!ConnectedAirVent) { return; }


        var cleanable = other.GetComponent<CleanableBase>();
        if (cleanable) { Teleport(cleanable); }

    }

    private void OnTriggerStay(Collider other)
    {
        var cleanVolume = other.GetComponent<CleaningVolume>();
        if (cleanVolume) { ExtendAirFlow(cleanVolume); }
    }
    private void OnTriggerExit(Collider other)
    {
        var cleanVolume = other.GetComponent<CleaningVolume>();
        if (cleanVolume)
        {
            if (vacuumExtension) { Destroy(vacuumExtension.gameObject); }
        }
    }

    private void ExtendAirFlow(CleaningVolume cleanVolume)
    {
        var parent = cleanVolume.transform.parent;
        if (!parent) { return; }


        var dist = Vector3.Distance(parent.transform.position, transform.position);
        if (dist > MinDistance)
        {
            if (vacuumExtension) { Destroy(vacuumExtension.gameObject); }
            return;
        }

        if (vacuumExtension) { return; }
        if (!VacuumExtensionPrefab) { return; }

        var parentCleaner = parent.GetComponent<CleaningEquipment>();
        if (!parentCleaner) { return; }


        var temp = Instantiate(VacuumExtensionPrefab, ConnectedAirVent.transform);

        temp.transform.localPosition = ExtensionOffest;
        temp.transform.localScale = ExtensionSize;

        var forward = ConnectedAirVent.transform.forward;
        var up = ConnectedAirVent.transform.up;
        temp.transform.rotation = Quaternion.LookRotation(forward, up);

        vacuumExtension = temp.GetComponent<CleaningEquipment>();
        parentCleaner.Extension = vacuumExtension;

    }

    private void Teleport(CleanableBase cleanable)
    {
        if (ignore)
        {
            if (cleanable == ignore)
            {
                ignore = null;
                return;
            }
        }

        var rb = cleanable.Rb;
        if (!rb) { return; }

        var col = cleanable.GetComponent<Collider>();
        if (!col) { return; }

        var velocity = rb.velocity;
        var angularVelocity = rb.angularVelocity;

        cleanable.gameObject.SetActive(false);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        var pos = ConnectedAirVent.transform.position;
        var forward = ConnectedAirVent.transform.forward.normalized;

        ConnectedAirVent.ignore = cleanable;
        cleanable.OnLeaveVolume.Invoke(cleanable);

        cleanable.transform.position = pos;
        cleanable.gameObject.SetActive(true);

        rb.velocity = forward * velocity.magnitude;
        rb.angularVelocity = angularVelocity;
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 2);

        var col = Color.yellow;
        col.a = 0.5f;
        Gizmos.color = col;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
