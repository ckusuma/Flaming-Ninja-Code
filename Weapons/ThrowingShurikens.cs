using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class ThrowingShurikens : MonoBehaviour {

    Input_Listeners IPL;

    [SerializeField]
    GameObject attachedGun;

    [SerializeField]
    GameObject interactableShurikenPrefab;

    GameObject instantiatedShuriken;

    [SerializeField]
    bool canThrow;
    bool vibrateHand;

    bool leftHand;

    NVRHand attachedHand;

    public AudioSource source;
    public AudioClip shurikenNoise;

    public Left_VR_Cont leftCont;
    public Right_VR_Cont rightCont;
    SwordUI swordUI;

    [SerializeField]
    float shootWaitTime = 1;

    float usageCost = 1;

    void Start()
    {
        IPL = Singleton_Service.GetSingleton<Input_Listeners>();
        swordUI = Singleton_Service.GetSingleton<SwordUI>();
        source = GetComponent<AudioSource>();
        attachedHand = GetComponent<NVRHand>();
      
        if (attachedGun.gameObject.activeSelf)
        {
            canThrow = false;
        }
        else
        {
            canThrow = true;
        }


    }

    void Update()
    {
        if (swordUI.FireIsDepleted())
        {
            return;
        }
        if (attachedGun.gameObject.activeSelf)
        {
            return;
        }
        if (attachedHand.HoldButtonDown)
        {
            if (attachedHand.CurrentlyInteracting == null)
            {
                VibrateController();
                PrepareShuriken();
                attachedHand.BeginInteraction(instantiatedShuriken.GetComponent<NVRInteractableItem>());
            }
        }
        else if (attachedHand.HoldButtonUp)
        {
            if (attachedHand.CurrentlyInteracting != null)
            {   
                if (attachedHand.CurrentlyInteracting.gameObject.CompareTag("Shuriken"))
                {
                    if (instantiatedShuriken != null)
                    {
                        if (attachedHand.CurrentlyInteracting == instantiatedShuriken.GetComponent<NVRInteractableItem>())
                        {
                            VibrateController();
                            attachedHand.EndInteraction(instantiatedShuriken.GetComponent<NVRInteractableItem>());
                            swordUI.DepleteFire(usageCost);
                            instantiatedShuriken.GetComponent<Rigidbody>().AddForce(instantiatedShuriken.GetComponent<Rigidbody>().velocity.normalized);
                            instantiatedShuriken.GetComponent<Shuriken>().SetThrown(true);
                            PlayShurikenNoise();
                            StartCoroutine("ShootShurikens");
                            StartCoroutine("ShootWaitTime");
                        }
                    }
                }

            }            
        }
    }

    void PrepareShuriken()
    {
        if (instantiatedShuriken == null)
        {
            instantiatedShuriken = Instantiate(interactableShurikenPrefab, this.transform.position, this.transform.rotation);
        }
        else
        {
            Destroy(instantiatedShuriken);
            instantiatedShuriken = Instantiate(interactableShurikenPrefab, this.transform.position, this.transform.rotation);
        }
        
    }

    void VibrateController()
    {
        if (this.gameObject.CompareTag("Left Hand"))
        {
            leftCont.PulseVibrate(10, 10, 0.005f, 1);
        }
        else
        {
            rightCont.PulseVibrate(10, 10, 0.005f, 1);
        }
    }

    IEnumerator ShootShurikens()
    {
        Rigidbody shurikenRB = instantiatedShuriken.GetComponent<Rigidbody>();
        //IF DOESN'T WORK, TRYING MAKING THIS AN IENUMERATOR TO WAIT FOR SHURIKEN TO GAIN VELOCITY

        yield return new WaitForSeconds(0.05f);
        PlayShurikenNoise();
        swordUI.DepleteFire(usageCost);
        if (interactableShurikenPrefab != null && instantiatedShuriken != null)
        {
            GameObject leftShuriken = Instantiate(interactableShurikenPrefab, instantiatedShuriken.transform.position, instantiatedShuriken.transform.rotation);
            attachedHand.BeginInteraction(leftShuriken.GetComponent<NVRInteractableItem>());
            attachedHand.EndInteraction(leftShuriken.GetComponent<NVRInteractableItem>());
            leftShuriken.GetComponent<Rigidbody>().AddForce(10 * (instantiatedShuriken.GetComponent<Rigidbody>().velocity + instantiatedShuriken.transform.right).normalized);
            leftShuriken.GetComponent<Shuriken>().SetThrown(true);
        }
        yield return new WaitForSeconds(0.05f);
        PlayShurikenNoise();
        swordUI.DepleteFire(usageCost);
        if (interactableShurikenPrefab != null && instantiatedShuriken != null)
        {
            GameObject leftShuriken = Instantiate(interactableShurikenPrefab, instantiatedShuriken.transform.position, instantiatedShuriken.transform.rotation);
            attachedHand.BeginInteraction(leftShuriken.GetComponent<NVRInteractableItem>());
            attachedHand.EndInteraction(leftShuriken.GetComponent<NVRInteractableItem>());
            leftShuriken.GetComponent<Rigidbody>().AddForce(10 * (instantiatedShuriken.GetComponent<Rigidbody>().velocity + instantiatedShuriken.transform.right).normalized);
            leftShuriken.GetComponent<Shuriken>().SetThrown(true);
        }
        instantiatedShuriken = null;

    }

    IEnumerator ShootWaitTime()
    {
        canThrow = false;
        yield return new WaitForSeconds(shootWaitTime);
        canThrow = true;
    }

    public void SetCanThrow (bool b)
    {
        canThrow = b;
    }

    public bool GetCanThrow()
    {
        return canThrow;
    }

    public void PlayShurikenNoise()
    {
        if (shurikenNoise != null && source != null)
        {
            source.clip = shurikenNoise;
            source.Play();
        }

    }
}
