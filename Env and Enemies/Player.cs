using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class Player : MonoBehaviour {

    SwordUI swordUI;

    public GameObject leftGun;
    public GameObject rightGun;

    public GameObject leftHand;
    public GameObject rightHand;

    public Left_VR_Cont leftCont;
    public Right_VR_Cont rightCont;

    public Transform playerPosition;
    float range = 15;
    float envRange = 30;

    public Transform environment;
    public Transform enemies;
    public Transform elements;

    AudioSource switchSource;
    public AudioClip toGunSound;
    public AudioClip toThrowSound;

    void Start()
    {
        swordUI = Singleton_Service.GetSingleton<SwordUI>();
        switchSource = GetComponent<AudioSource>();
        //StartCoroutine("EnableSurroundingObjects");
    }

    void Update()
    {
        if (leftCont == null || rightCont == null)
        {
            return;
        }
        if (rightCont.controller.GetPressDown(rightCont.rightJoystick))
        {
            SwitchModes();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            swordUI.DamagePlayer(other.GetComponent<Enemy>().GetDamage());
        }
        if (other.CompareTag("Spike"))
        {
            swordUI.DamagePlayer(10);
        }
    }

    public void SwitchModes()
    {
        if (leftGun.activeSelf)
        {
            switchSource.clip = toThrowSound;
            switchSource.Play();
        }
        else
        {
            switchSource.clip = toGunSound;
            switchSource.Play();
        }
        leftGun.SetActive(!leftGun.activeSelf);
        rightGun.SetActive(!rightGun.activeSelf);
        leftHand.GetComponent<ThrowingShurikens>().SetCanThrow(!leftHand.GetComponent<ThrowingShurikens>().GetCanThrow());
        leftHand.GetComponent<NVRHand>().EndInteraction(null);
        rightHand.GetComponent<ThrowingShurikens>().SetCanThrow(!rightHand.GetComponent<ThrowingShurikens>().GetCanThrow());
        rightHand.GetComponent<NVRHand>().EndInteraction(null);
    }

    IEnumerator EnableSurroundingObjects()
    {
        while (true)
        {
            if (environment != null)
            {
                foreach (Transform piece in environment)
                {
                    if (Vector3.Distance(playerPosition.position, piece.position) <= envRange)
                    {
                        piece.gameObject.SetActive(true);
                    }
                    else
                    {
                        piece.gameObject.SetActive(false);
                    }
                }
            }

            if (enemies != null)
            {
                foreach (Transform enemy in enemies)
                {
                    if (Vector3.Distance(playerPosition.position, enemy.position) <= range)
                    {
                        enemy.gameObject.SetActive(true);
                    }
                    else
                    {
                        enemy.gameObject.SetActive(false);
                    }
                }
            }

            if (elements != null)
            {
                foreach (Transform element in elements)
                {
                    if (Vector3.Distance(playerPosition.position, element.position) <= range)
                    {
                        element.gameObject.SetActive(true);
                    }
                    else
                    {
                        element.gameObject.SetActive(false);
                    }
                }
            }

            yield return new WaitForSeconds(1);
        }

    }
}
