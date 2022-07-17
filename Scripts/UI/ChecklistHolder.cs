using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChecklistHolder : MonoBehaviour
{
    List<ChecklistSlot> slots = new List<ChecklistSlot>();

    public ChecklistSlot SlotPrefab;

    public int Yoffset = 100;

    public float OffsetSize = 0.0001f;

    public ScriptableTaskList listOfTasks;

    private void Start()
    {
        SetupTaskList(listOfTasks);
    }

    public void SetupTaskList(ScriptableTaskList newList)
    {
        if (!newList) { return; }
        if (!SlotPrefab) { return; }

        if (slots.Count > 0) { ClearSlots(); }

        listOfTasks = newList;

        var inactiveOffset = 0;

        for (int i = 0; i < listOfTasks.Items.Count; i++)
        {
            var item = listOfTasks.Items[i];
            if (!item) { continue; }

            if (item.TaskStatus == Team14.TaskStatus.Inactive)
            {
                item.RegisterActive(UpdateUI);
                inactiveOffset++;
                continue; 
            }

            var slot = Instantiate(SlotPrefab, transform);
            slot.SetupSlot(item);

            var rectTransform = slot.GetComponent<RectTransform>();
            if (!rectTransform) { continue; }

            var xpos = rectTransform.anchoredPosition.x;
            var ypos = rectTransform.anchoredPosition.y - (OffsetSize * (float)(Yoffset * (i-inactiveOffset)));
            rectTransform.anchoredPosition = new Vector2(xpos, ypos);

            item.Register(UpdateUI);
            slots.Add(slot);
        }
    }

    private void UpdateUI()
    {
        if (this == null) return;
        SetupTaskList(listOfTasks);
    }

    private void ClearSlots()
    {
        if (!listOfTasks) { return; }
        for (int i = slots.Count - 1; i >= 0; i--)
        {
            var item = slots[i];
            if (!item) { continue; }

            var task = listOfTasks.Items.FirstOrDefault(q => q.GetInstanceID() == item.TaskID);
            if (!task) { continue; }

            task.UnRegister(UpdateUI);
            task.UnregisterActive(UpdateUI);
            Destroy(item.gameObject);
        }
        slots.Clear();
    }
}
