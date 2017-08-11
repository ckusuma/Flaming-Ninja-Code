using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Element {

    public GameObject particles;
    BoxCollider thisCollider;

    public AudioSource fireStartSound;
    public AudioSource fireplaceSound;

    float fireWaitTime = 20;

    public void Start()
    {
        IPL = Singleton_Service.GetSingleton<Input_Listeners>();
        playerSword = Singleton_Service.GetSingleton<SwordUI>();
        type = "Fire";

        thisCollider = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Left Hand") || other.CompareTag("Right Hand") || other.CompareTag("Player"))
        {
            fireStartSound.Play();
            playerSword.ReplenishFire(100);
            StartCoroutine("DisableFire");
            GameObject LH = GameObject.FindGameObjectWithTag("Left Hand");
            GameObject RH = GameObject.FindGameObjectWithTag("Right Hand");
            if (LH != null)
            {
                LH.GetComponent<Left_VR_Cont>().PulseVibrate(10, 10, 0.01f, 1);
            }
            if (RH != null)
            {
                RH.GetComponent<Right_VR_Cont>().PulseVibrate(10, 10, 0.01f, 1);
            }

        }
    }



    IEnumerator DisableFire()
    {
        thisCollider.enabled = false;
        particles.SetActive(false);
        if (fireplaceSound.isPlaying)
        {
            fireplaceSound.Stop();
        }
        yield return new WaitForSeconds(fireWaitTime);
        thisCollider.enabled = true;
        particles.SetActive(true);
        fireStartSound.Play();
        fireplaceSound.Play();
    }

}
