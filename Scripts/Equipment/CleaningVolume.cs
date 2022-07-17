using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CleaningVolume : MonoBehaviour
{
    public List<CleanableBase> overlappingObjects = new List<CleanableBase>();

    public UnityEvent<GameObject> OnAttatchToOrigin = new UnityEvent<GameObject>();
    public UnityEvent<GameObject> OnDetatchFromOrigin = new UnityEvent<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other) { return; }
        var cleanable = other.GetComponent<CleanableBase>();
        if (!cleanable) { return; }

        if(overlappingObjects.Count>0 && overlappingObjects.Contains(cleanable)) { return; }

        overlappingObjects.Add(cleanable);
        cleanable.OnDestruction.AddListener(DestroyOverlappingObject);
        cleanable.OnLeaveVolume.AddListener(RemoveFromVolume);
        cleanable.OnAttatchToOrigin.AddListener(OnAttatchToOrigin.Invoke);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other) { return; }
        var cleanable = other.GetComponent<CleanableBase>();
        if (!cleanable) { return; }

        if (overlappingObjects.Count > 0 && overlappingObjects.Contains(cleanable)) { return; }

        overlappingObjects.Add(cleanable);
        cleanable.OnDestruction.AddListener(DestroyOverlappingObject);
        cleanable.OnLeaveVolume.AddListener(RemoveFromVolume);
        cleanable.OnAttatchToOrigin.AddListener(OnAttatchToOrigin.Invoke);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other) { return; }
        var cleanable = other.GetComponent<CleanableBase>();
        if (!cleanable) { return; }

        if (cleanable.Attatched) { return; }
        RemoveFromVolume(cleanable);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < overlappingObjects.Count; i++)
        {
            var item = overlappingObjects[i];
            if (!item) { continue; }
            RemoveFromVolume(item);
        }
    }

    public void RemoveFromVolume(CleanableBase cleanable)
    {
        if (overlappingObjects.Contains(cleanable))
        {
            RemoveFromOverlapping(cleanable);
            cleanable.ResetSize();

            if (!cleanable.Attatched) { return; }
            OnDetatchFromOrigin.Invoke(cleanable.gameObject);
            cleanable.Attatched = false;
        }
    }

    private void RemoveFromOverlapping(CleanableBase cleanable)
    {
        cleanable.OnDestruction.RemoveListener(DestroyOverlappingObject);
        cleanable.OnLeaveVolume.RemoveListener(RemoveFromVolume);
        cleanable.OnAttatchToOrigin.RemoveListener(OnAttatchToOrigin.Invoke);
        overlappingObjects.Remove(cleanable);
    }

    private void DestroyOverlappingObject(CleanableBase cleanable)
    {
        RemoveFromOverlapping(cleanable);
        Destroy(cleanable.gameObject);
    }
}
