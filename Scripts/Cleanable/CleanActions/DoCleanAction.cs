using UnityEngine;

public class DoCleanAction : ScriptableObject, IAction<Equipment, CleanableBase>
{
    public virtual void DoAction(Equipment owner, CleanableBase target)
    {
        throw new System.NotImplementedException();
    }
}
