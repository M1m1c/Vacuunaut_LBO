using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmentStats", menuName = "Equipment/EquipmentStatsBase")]
public class EquipmentStatsBase : ScriptableObject
{
    public Vector3 EquipedPositionOffset;
    public int MaxCapacity=10;
    [Range(0.1f, 2f)]
    public float TimeBetweenShots = 0.5f;

    public SettingsSet[] Sets;
}

[System.Serializable]
public struct SettingsSet
{
    public EquipmentAction PrimaryAction;
    public EquipmentAction SecondaryAction;
}

