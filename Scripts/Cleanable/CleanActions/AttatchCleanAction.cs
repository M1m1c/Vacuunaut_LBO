using UnityEngine;

[CreateAssetMenu(fileName = "AttatchCleanAction", menuName = "CleanActions/AttatchCleanAction")]
public class AttatchCleanAction : DoCleanAction
{
    public override void DoAction(Equipment owner, CleanableBase target)
    {
        target.ResetSize();

        if (target.Attatched) { return; }
        target.Attatched = true;
        target.OnAttatchToOrigin.Invoke(target.gameObject);

    }
}
