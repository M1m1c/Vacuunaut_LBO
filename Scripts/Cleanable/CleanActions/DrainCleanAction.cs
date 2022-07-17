using UnityEngine;

[CreateAssetMenu(fileName = "DrainCleanAction", menuName = "CleanActions/DrainCleanAction")]
public class DrainCleanAction : DoCleanAction
{
    [Range(1f, 10f)]
    public float CleanTime = 2f;

    public override void DoAction(Equipment owner, CleanableBase target)
    {
        target.DrainTime = Mathf.Clamp(target.DrainTime - Time.deltaTime, 0f, 10f);
        if (target.DrainTime <= 0.0f) { target.OnDestruction.Invoke(target); }

    }
}
