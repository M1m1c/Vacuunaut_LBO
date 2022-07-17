using UnityEngine;

[CreateAssetMenu(fileName = "DestroyCleanAction", menuName = "CleanActions/DestroyCleanAction")]
public class DestroyCleanAction : DoCleanAction
{
    public override void DoAction(Equipment owner, CleanableBase target)
    {
        target.OnDestruction.Invoke(target);
        
        if (!owner) { return; }

        var cleaner = owner.GetComponent<CleaningEquipment>();
        if (!cleaner) { return; }

        cleaner.CurrentCapacity+=target.RecycleValue;
    }
}
