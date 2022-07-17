using UnityEngine;

public class EquipmentAction : ScriptableObject, IAction<Equipment>
{
    public int StartupSoundIndex = -1;
    public int LoopSoundIndex = -1;
    public int EndSoundIndex = -1;
    public virtual void DoAction(Equipment owner)
    {
        throw new System.NotImplementedException();
    }

    public virtual void Initalise(Equipment owner)
    {

    }
}
