using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHitMarker : MonoBehaviour {

    bool isHit;

    Fire currElementComponent;

    SwordUI playerSword;

    Enemy attachedEnemy;
    GameObject particlesRef;
    public GameObject particles;

    Left_VR_Cont leftController;
    Right_VR_Cont rightController;

    void Start()
    {
        isHit = false;
        if (this.transform.parent != null)
        {
            attachedEnemy = this.transform.parent.GetComponent<Enemy>();
        }

        currElementComponent = GetComponent<Fire>();
        playerSword = Singleton_Service.GetSingleton<SwordUI>();
    }

    //if hit by the plyaer sword, the marker is marked as hit
    void OnCollisionEnter(Collision other)
    {

        if (other.collider.CompareTag("Player Sword") || other.collider.CompareTag("Shuriken"))
        {
            Debug.Log("Collided");
            leftController = GameObject.FindGameObjectWithTag("Left Hand").GetComponent<Left_VR_Cont>();
            rightController = GameObject.FindGameObjectWithTag("Right Hand").GetComponent<Right_VR_Cont>();

            rightController.PulseVibrate(10, 10, 0.001f, 1);

            SetHitStatus(true);
            if (attachedEnemy != null)
            {
                if (!attachedEnemy.HitAlreadySignaled())
                {
                    attachedEnemy.SignalAHit(this.gameObject);
                    ContactPoint[] points = other.contacts;
                    foreach (ContactPoint point in points)
                    {
                        if (point.otherCollider.CompareTag("Player Sword") || point.otherCollider.CompareTag("Shuriken"))
                        {
                            attachedEnemy.StartCriticalHit(point.point);
                        }
                    }
                }
            }
            else
            {
                ContactPoint[] points = other.contacts;
                foreach (ContactPoint point in points)
                {
                    if (point.otherCollider.CompareTag("Player Sword") || point.otherCollider.CompareTag("Shuriken"))
                    {
                        attachedEnemy.StartCriticalHit(point.point);
                    }
                }
            }
        }
    }

    public void SetHitStatus(bool b)
    {
        isHit = b;
    }

    public bool GetHitStatus()
    {
        return isHit;
    }
}
