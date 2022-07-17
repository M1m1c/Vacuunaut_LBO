using UnityEngine;

[CreateAssetMenu(fileName = "WithinCapacityDoClean", menuName = "CleanableConditions/WithinCapacityDoClean")]
public class WithinCapacityDoClean : DoCleanConditonBase
{
    public AudioClip fullCapacitySound;
 
    public override bool CheckCondition(ConditionInfo conditionInfo)
    {
        var retval = conditionInfo.CurrentCap < conditionInfo.MaxCap;

        if (!retval && fullCapacitySound)
        {
            var audioManager = AudioManager.Instance;
            if (audioManager)
            {
                audioManager.PlayAudioNoOverlap(fullCapacitySound, AudioType.SFX);
            }
        }
        return retval;
    }
}
