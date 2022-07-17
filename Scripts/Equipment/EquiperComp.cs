using System;
using System.Collections;
using Team14;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquiperComp : MonoBehaviour
{
    public Equipment StartingEquipment;
    public Equipment CurrentEquipment { get; private set; }

    public Vector3 EquipedPosition;

    private EquipmentSway swayComp;

    private bool isPrimaryClick = false;

    private bool holdingPrimaryInput;
    private bool holdingSecondaryInput;
    private bool primaryToggle;
    private bool secondaryToggle;

    public void Equip(Equipment equipment)
    {
        if (CurrentEquipment)
        {
            Destroy(CurrentEquipment.gameObject);
            CurrentEquipment = null;
        }

        var instance = Instantiate(equipment, transform);
        instance.Setup();


        var mat = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        var heldPos = mat.MultiplyPoint( EquipedPosition + instance.EquipedPositionOffset);
        instance.transform.position = heldPos;

        CurrentEquipment = instance;
        if (!CurrentEquipment) { return; }
        swayComp = CurrentEquipment.GetComponentInChildren<EquipmentSway>();
    }

    public void SwitchSetting(InputAction.CallbackContext context)
    {
        if (!CurrentEquipment) { return; }
        if (context.performed) { CurrentEquipment.SwitchSetting(); }
    }

    public void InputPrimary(InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            StartCoroutine(PrimaryClick());           
            Settings.LockCursorToGame();
        }
        if (context.canceled) 
        {
            StopAllCoroutines();
            isPrimaryClick = false;
            holdingPrimaryInput = false; 
        }
    }

    public void InputSecondary(InputAction.CallbackContext context)
    {
        if (context.performed) { holdingSecondaryInput = true; }
        if (context.canceled) { holdingSecondaryInput = false; }
        Settings.LockCursorToGame();
    }

    public void TriggerJumpSway()
    {
        if (!swayComp) { return; }
        swayComp.SetJumpOffsetToMax();
    }

    public void TriggerSwayReset()
    {
        if (!swayComp) { return; }
        swayComp.ResetJumping();
    }

    private void Start()
    {
        if (StartingEquipment) { Equip(StartingEquipment); }
    }

    private void Update()
    {
        if (!CurrentEquipment) { return; }
        var primaryBypass = CurrentEquipment.ShouldBypassPrimaryHold();

        if (primaryBypass == true) { primaryBypass = isPrimaryClick == false; }

        var priority = PerformHoldingAction(
            holdingPrimaryInput,
            ref primaryToggle,
            primaryBypass,
            DoPrimary,
            StopPrimary);

        if (!priority || (primaryBypass && !holdingPrimaryInput))
        {
            PerformHoldingAction(
           holdingSecondaryInput,
           ref secondaryToggle,
           false,
           DoSecondary,
           StopSecondary);
        }
    }

    private bool PerformHoldingAction(bool holding, ref bool toggle, bool bypass, Action active, Action deactive)
    {
        var retval = false;
        if (holding || bypass)
        {
            if (!toggle) { toggle = true; }
            active.Invoke();
            retval = true;
        }
        else if (toggle)
        {
            toggle = false;
            deactive.Invoke();
        }
        return retval;
    }

    private void DoPrimary()
    {
        if (!CurrentEquipment) { return; }
        CurrentEquipment.DoPrimaryAction();
    }

    private void StopPrimary()
    {
        if (!CurrentEquipment) { return; }
        CurrentEquipment.StopPrimaryAction();
    }

    private void DoSecondary()
    {
        if (!CurrentEquipment) { return; }
        CurrentEquipment.DoSecondaryAction();
    }

    private void StopSecondary()
    {
        if (!CurrentEquipment) { return; }
        CurrentEquipment.StopSecondaryAction();
    }

    private IEnumerator PrimaryClick()
    {
        isPrimaryClick = true;
        yield return new WaitForSeconds(0.15f);
        isPrimaryClick = false;
        holdingPrimaryInput = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

       var mat= Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

        Gizmos.DrawWireSphere(mat.MultiplyPoint(EquipedPosition), 0.5f);
    }
}
