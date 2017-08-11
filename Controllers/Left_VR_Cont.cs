using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Left_VR_Cont : MonoBehaviour
{

    public Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    public Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;

    public Valve.VR.EVRButtonId aButton = Valve.VR.EVRButtonId.k_EButton_A;

    public SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); }/*get { return SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost)); }*/ }
    private SteamVR_TrackedObject trackedObj;

    Input_Listeners IPL;
    public Sheath sheath;

    // Use this for initialization
    void Start()
    {
        IPL = Singleton_Service.GetSingleton<Input_Listeners>();
        //trackedObj = GetComponent<SteamVR_TrackedObject>();

    }

    // Update is called once per frame
    void Update()
    {
        if (controller.GetPressDown(triggerButton))
        { IPL.SetLeftTriggerInteracting(true); }
        if (controller.GetPressUp(triggerButton))
        { IPL.SetLeftTriggerInteracting(false); 
        }
        if (controller.GetPressDown(gripButton))
        {
            IPL.SetLeftGripInteracting(true);
        }
        if (controller.GetPressUp(gripButton))
        {
            IPL.SetLeftGripInteracting(false);
        }

    }

    public SteamVR_Controller.Device GetLeftController()
    {
        return controller;
    }

    public void ContinuousVibrate(float length, float strength)
    {
        StartCoroutine(ContinuousVibration(length, strength));
    }

    public void PulseVibrate(int vibrationCount, float vibrationLength, float gapLength, float strength)
    {
        StartCoroutine(PulsedVibration(vibrationCount, vibrationLength, gapLength, strength));
    }

    //length is how long the vibration should go for
    //strength is vibration strength from 0-1
    public IEnumerator ContinuousVibration(float length, float strength)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            controller.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
        }
        yield return null;
    }

    //vibrationCount is how many vibrations
    //vibrationLength is how long each vibration should go for
    //gapLength is how long to wait between vibrations
    //strength is vibration strength from 0-1
    public IEnumerator PulsedVibration(int vibrationCount, float vibrationLength, float gapLength, float strength)
    {
        strength = Mathf.Clamp01(strength);
        for (int i = 0; i < vibrationCount; i++)
        {
            if (i != 0) yield return new WaitForSeconds(gapLength);
            yield return StartCoroutine(ContinuousVibration(vibrationLength, strength));
        }
    }

    void OnEnable()
    {
        Singleton_Service.RegisterSingletonInstance<Left_VR_Cont>(this);
        if (sheath != null)
        {
            sheath.leftController = this;
        }
        trackedObj = GetComponent<SteamVR_TrackedObject>();


    }
    void OnDisable()
    {
        Singleton_Service.UnregisterSingletonInstance<Left_VR_Cont>(this);
    }

}
