using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumUIController : MonoBehaviour
{

    public AudioManager audioManager;

    [Header("VacuumSwitch")]
    public GameObject VacuumSwitch;
    public Vector3 A_SwitchPos;
    public Vector3 B_SwitchPos;
    public Quaternion A_SwitchRot;
    public Quaternion B_SwitchRot;
    public AudioClip SwitchClip;
    private bool AorB = false;


    [Header("AirVFX")]
    public Material AirVFXMat;

    [Header("LightVFX")]
    public Material LightMat;

    private bool lightActive;
    private float lightIntensity;
    private float maxIntensity = 0.007f;

    [Header("CapaciyVFX")]
    public Material CapacityMat;

    private void Start()
    {

        if (AirVFXMat)
        {
            AirVFXMat.SetInt("Boolean_89be80007e7d4da9a910189877966392", 0);
        }

        if (LightMat)
        {
            LightMat.SetFloat("Vector1_84686bd3f16844bdbbba5637ba795355", 0f);
        }
    }
    private void Update()
    {
        if (!LightMat) { return; }

        if (lightActive)
        { lightIntensity = Mathf.Clamp(lightIntensity + (Time.deltaTime*maxIntensity), 0f, maxIntensity); }
        else
        { lightIntensity = Mathf.Clamp(lightIntensity - (Time.deltaTime*maxIntensity *5f), 0f, maxIntensity); }

        LightMat.SetFloat("Vector1_84686bd3f16844bdbbba5637ba795355", lightIntensity);
    }

    public void ActivateVacuumSwitch()
    {
        if (!VacuumSwitch) { return; }

        if (AorB)
        {
            AorB = false;
            VacuumSwitch.transform.localPosition = A_SwitchPos;
            VacuumSwitch.transform.localRotation = A_SwitchRot;

            ChangeLightSet(0f);
        }
        else
        {
            AorB = true;
            VacuumSwitch.transform.localPosition = B_SwitchPos;
            VacuumSwitch.transform.localRotation = B_SwitchRot;

            ChangeLightSet(1f);
        }

        if (!SwitchClip) { return; }
        if (!audioManager) { return; }
        audioManager.PlayAudio(SwitchClip, AudioType.SFX, transform.position);
    }

    private void ChangeLightSet(float set)
    {
        if (LightMat)
        {
            LightMat.SetFloat("Vector1_47bc535176e44d3194fa8c35967e8090", set);
        }
    }

    public void SetAirVFX(bool active, float speed)
    {
        AirVFXMat.SetInt("Boolean_89be80007e7d4da9a910189877966392", active ? 1 : 0);
        AirVFXMat.SetFloat("Vector1_c097ab41f75a421f96c57a6946ad3f26", speed);
    }

    public void SetLightVFX(bool active, float primOrSec)
    {
        lightActive = active;
        LightMat.SetFloat("Vector1_9704d01fb5114cf5adb774f73eed153a", primOrSec);
    }

    public void SetCapacityVFX(float value)
    {
        if (CapacityMat)
        {
            CapacityMat.SetFloat("_BarValue", value);
        }
    }
}
