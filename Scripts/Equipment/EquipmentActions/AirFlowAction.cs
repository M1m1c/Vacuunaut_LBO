
public class AirFlowAction : EquipmentAction
{
    public DoCleanConditonBase[] Condition;

    public DoCleanAction CleanAction;

    protected void CheckDoClean(Equipment owner, ConditionInfo info, CleanableBase obj)
    {
        if (!owner) { return; }
        if (info==null) { return; }
        if (!obj) { return; }

        var priority = DoCleanableSpecificAciton(owner, obj, info);

        if (priority) { return; }

        DoCleanAction(owner, obj, info);
    }

    protected bool CheckCondition(ConditionInfo info)
    {
        var retval = true;
        if (info != null) 
        {
            if (Condition != null)
            {
                if (Condition.Length > 0)
                {
                    foreach (var cond in Condition)
                    {
                        if (!cond) { continue; }
                        if (!cond.CheckCondition(info))
                        {
                            retval = false;
                            break;
                        }
                    }
                }
            }           
        }    
        return retval;
    }


    private bool DoCleanableSpecificAciton(Equipment owner, CleanableBase obj, ConditionInfo info)
    {
        var retval = false;
        if (obj.CleanAction)
        {
            if (obj.CleanableConditon.Length > 0)
            {
                var correct = 0;
                foreach (var cond in obj.CleanableConditon)
                {
                    if (!cond.CheckCondition(info)) { break; }
                    correct++;
                }

                if (correct == obj.CleanableConditon.Length)
                {
                    retval = true;
                    obj.CleanAction.DoAction(owner, obj);
                }
            }
        }
        return retval;
    }

    private void DoCleanAction(Equipment owner, CleanableBase obj, ConditionInfo info)
    {
        if(info == null) { return; }
        if (!CheckCondition(info)) { return; }

        if (!CleanAction) { return; }

        CleanAction.DoAction(owner, obj);
    }

}
