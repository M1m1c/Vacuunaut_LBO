using UnityEngine;

public class DoCleanConditonBase : ScriptableObject
{
    public virtual bool CheckCondition(ConditionInfo conditionInfo)
    {
        return false;
    }
}

public class ConditionInfo
{
    public ConditionInfo(
    EDirection eDirection,
    Vector3 equipmentOrigin,
    Vector3 objPosition,
    Vector3 playerPos,
    float destructionDistance,
    int maxCapacity,
    int currentCapacity,
    int addValue)
    {
        Edir = eDirection;
        EquipmentOrigin = equipmentOrigin;
        ObjPosition = objPosition;
        PlayerPos = playerPos;
        DestructionDistance = destructionDistance;
        MaxCap = maxCapacity;
        CurrentCap = currentCapacity;
        CapAddValue = addValue;
    }

    public EDirection Edir { get; private set; }
    public Vector3 EquipmentOrigin { get; private set; }
    public Vector3 ObjPosition { get; private set; }
    public Vector3 PlayerPos { get; private set; }
    public float DestructionDistance { get; private set; }

    public int MaxCap { get; private set; }
    public int CurrentCap { get; private set; }
    public int CapAddValue { get; private set; }
}