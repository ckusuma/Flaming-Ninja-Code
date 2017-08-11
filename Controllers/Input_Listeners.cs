using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Listeners : MonoBehaviour {

    [SerializeField]
    bool leftTriggerInteracting;
    [SerializeField]
    bool rightTriggerInteracting;
    [SerializeField]
    bool leftGripInteracting;
    [SerializeField]
    bool rightGripInteracting;
    [SerializeField]
    Vector3 rightControllerVelocity;

    // Use this for initialization
    void Start () {
        leftTriggerInteracting = false;
        rightTriggerInteracting = false;

        leftGripInteracting = false;
        rightGripInteracting = false;
    }
	
    public bool GetLeftTriggerInteracting()
    {
        return leftTriggerInteracting;
    }

    public void SetLeftTriggerInteracting (bool b)
    {
        leftTriggerInteracting = b;
    }

    public bool GetRightTriggerInteracting()
    {
        return rightTriggerInteracting;
    }

    public void SetRightTriggerInteracting(bool b)
    {
        rightTriggerInteracting = b;
    }

    public bool GetLeftGripInteracting()
    {
        return leftGripInteracting;
    }

    public void SetLeftGripInteracting(bool b)
    {
        leftGripInteracting = b;
    }

    public bool GetRightGripInteracting()
    {
        return rightGripInteracting;
    }

    public void SetRightGripInteracting(bool b)
    {
        rightGripInteracting = b;
    }

    public Vector3 GetRightControllerVelocity()
    {
        return rightControllerVelocity;
    }

    public void SetRightControllerVelocity(Vector3 vel)
    {
        rightControllerVelocity = vel;
    }

    void OnEnable()
    {
        Singleton_Service.RegisterSingletonInstance(this);
    }
    void OnDisable()
    {
        Singleton_Service.UnregisterSingletonInstance(this);
    }

}
