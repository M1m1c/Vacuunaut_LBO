using UnityEngine;
using UnityEngine.Events;

public class CleanableBase : MonoBehaviour
{
    public bool Attatched { get; set; }
    public float OriginalMass { get; private set; }
    public float DrainTime { get; set; }

    public Rigidbody Rb { get; private set; }

    public int RecycleValue = 1;

    public DoCleanConditonBase[] CleanableConditon;
    public DoCleanAction CleanAction;
    public UnityEvent<CleanableBase> OnDestruction = new UnityEvent<CleanableBase>();
    public UnityEvent<CleanableBase> OnLeaveVolume = new UnityEvent<CleanableBase>();
    public UnityEvent<GameObject> OnAttatchToOrigin = new UnityEvent<GameObject>();
    public UnityEvent<GameObject> OnDetatchFromOrigin = new UnityEvent<GameObject>();


    private MeshRenderer meshRenderer;

    public Vector3 StartSize { get; private set; }

    void Start()
    {
        Setup();

        StartSize = transform.localScale;

        if (CleanAction)
        {
            if (CleanAction is DrainCleanAction drainClean)
            {
                DrainTime = drainClean.CleanTime;
            }
        }
        meshRenderer = GetComponent<MeshRenderer>();
        if (!Rb) { return; }
        OriginalMass = Rb.mass;
    }

    public virtual void Setup()
    {
        Rb = GetComponent<Rigidbody>();
        if (!Rb)
        {
            Rb = gameObject.AddComponent<Rigidbody>();
            Rb.isKinematic = false;
            Rb.useGravity = true;
        }
    }

    public virtual void DoClean(Equipment equipment, CleanableBase cleanable)
    {
        if (!CleanAction) { return; }
        CleanAction.DoAction(equipment, cleanable);
    }

    public void ResetSize()
    {
        transform.localScale = StartSize;
    }

    private void OnCollisionEnter(Collision collision)
    {      
        if (!meshRenderer) { return; }
        if (meshRenderer.enabled) { return; }
        meshRenderer.enabled = true;
    }
}
