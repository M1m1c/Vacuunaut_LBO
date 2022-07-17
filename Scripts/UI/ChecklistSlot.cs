using Team14;
using UnityEngine;
using UnityEngine.UI;

public class ChecklistSlot : MonoBehaviour
{
    public Text Description;
    public Image CheckMark;
    public Image Backgorund;

    public int TaskID { get; private set; }

    public void SetupSlot(ScriptableTaskBase task)
    {
        Description.text = task.Description + task.GetProgressText();
        CheckMark.enabled = task.TaskStatus == TaskStatus.Completed ? true : false;
        TaskID = task.GetInstanceID();
    }

    public void FinishTask()
    {
        CheckMark.enabled = true;
    }
}
