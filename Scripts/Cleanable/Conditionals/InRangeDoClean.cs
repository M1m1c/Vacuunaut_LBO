using UnityEngine;

[CreateAssetMenu(fileName = "InRangeDoClean", menuName = "CleanableConditions/InRangeDoClean")]

public class InRangeDoClean : DoCleanConditonBase
{
    public override bool CheckCondition(ConditionInfo conditionInfo)
    {
        var origin = conditionInfo.EquipmentOrigin;
        var playerPos = conditionInfo.PlayerPos;
        var currentPos = conditionInfo.ObjPosition;
        var range = conditionInfo.DestructionDistance;

        return Vector3.Distance(origin, currentPos) < range || Vector3.Distance(playerPos, currentPos) < range;
    }
}
