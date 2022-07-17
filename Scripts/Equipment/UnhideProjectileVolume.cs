using UnityEngine;

public class UnhideProjectileVolume : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        var cleanable = other.GetComponent<CleanableBase>();
        if (!cleanable) { return; }

        var mesh = cleanable.GetComponent<MeshRenderer>();
        if (!mesh) { return; }

        if (mesh.enabled) { return; }
        mesh.enabled = true;
    }
}
