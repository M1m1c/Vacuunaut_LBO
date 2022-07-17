using UnityEngine;
using UnityEngine.Events;

public interface IEquipment
{
    abstract void Setup();
    abstract void DoPrimaryAction();
    abstract void DoSecondaryAction();
}

public class Equipment : MonoBehaviour, IEquipment
{
    public EquipmentStatsBase Stats;

    public Equipment Extension { get; set; }

    public UnityEvent SwitchSettingEvent = new UnityEvent();
    public UnityEvent<bool, float> AirVFXEvent = new UnityEvent<bool, float>();
    public UnityEvent<bool, float> LightVFXEvent = new UnityEvent<bool, float>();

    public Vector3 EquipedPositionOffset { get; private set; }
    protected EquipmentAction primaryAction;
    protected EquipmentAction secondaryAction;
    protected int settingsIndex = 2;

    protected VFXSetting vfxSetting;
    protected float airVFXSpeed = 5f;

    public virtual void DoPrimaryAction()
    {
        if (primaryAction == null) { return; }
        primaryAction.DoAction(this);

        SetVFX(true,-airVFXSpeed,0);

        if (!Extension) { return; }
        Extension.DoPrimaryAction();
    }

   

    public virtual void DoSecondaryAction()
    {
        if (secondaryAction == null) { return; }
        secondaryAction.DoAction(this);

        SetVFX(true, airVFXSpeed,1);

        if (!Extension) { return; }
        Extension.DoSecondaryAction();
    }

    public virtual void StopPrimaryAction()
    {
        SetVFX(false, 0,0);
    }

    public virtual void StopSecondaryAction()
    {
        SetVFX(false, 0,1);
    }

    public virtual bool ShouldBypassPrimaryHold()
    {
        return false;
    }

    private void Start()
    {
        Setup();
        vfxSetting = new VFXSetting();
        vfxSetting.PrimOrSec = -1f;
    }

    public virtual void Setup()
    {
        if (!Stats) { return; }

        EquipedPositionOffset = Stats.EquipedPositionOffset;

        foreach (var set in Stats.Sets)
        {
            set.PrimaryAction.Initalise(this);
            set.SecondaryAction.Initalise(this);
        }

        SwitchSetting();
    }

    public void SwitchSetting()
    {
        if (Stats.Sets.Length <= 0) { return; }

        settingsIndex++;
        if (settingsIndex >= Stats.Sets.Length || settingsIndex < 0) { settingsIndex = 0; }

        var statsSetting = Stats.Sets[settingsIndex];
        if (!statsSetting.PrimaryAction || !statsSetting.SecondaryAction) { return; }

        SwitchSettingEvent.Invoke();
        primaryAction = statsSetting.PrimaryAction;
        secondaryAction = statsSetting.SecondaryAction;
    }

    protected void SetVFX(bool active, float speed, float primOrSec)
    {
        if (vfxSetting.Active != active ||
            vfxSetting.CurrentSpeed != speed ||
            vfxSetting.PrimOrSec != primOrSec ||
            active == false)
        {
            AirVFXEvent.Invoke(active, speed);
            LightVFXEvent.Invoke(active, primOrSec);
            vfxSetting.Active = active;
            vfxSetting.CurrentSpeed = speed;
            vfxSetting.PrimOrSec = primOrSec;
        }
    }

    protected struct VFXSetting
    {
        public bool Active { get; set; }
        public float CurrentSpeed { get; set; }
        public float PrimOrSec { get; set; }
    }
}

