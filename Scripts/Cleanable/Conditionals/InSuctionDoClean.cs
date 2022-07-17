using UnityEngine;

[CreateAssetMenu(fileName = "InSuctionDoClean", menuName = "CleanableConditions/InSuctionDoClean")]
public class InSuctionDoClean : DoCleanConditonBase
{
    public override bool CheckCondition(ConditionInfo conditionInfo)
    {
        return conditionInfo.Edir == 0;
    }
}
