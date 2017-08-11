using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class Sheath : MonoBehaviour {

    Input_Listeners IPL;

    public GameObject player;

    public GameObject swordPrefab;
    public Transform swordPosition;

    public Transform sheathPoint;

    GameObject currSwordInSheath;

    [SerializeField]
    bool swordIsActive;
    [SerializeField]
    bool sheathGracePeriodIsActive;

    float sheathGracePeriod = 5;

    bool rightTriggerPressedOnEnter;

    public Left_VR_Cont leftController;
    public Right_VR_Cont rightController;

    AudioSource sheathSource;
    public AudioClip drawSound;
    public AudioClip sheathSound;

    public Camera cam;

    bool wantToDrawSword;

    void Start()
    {
        IPL = Singleton_Service.GetSingleton<Input_Listeners>();
        sheathSource = GetComponent<AudioSource>();
        SpawnNewSword();
    }

    void Update()
    {
        Quaternion temp = Quaternion.Euler(Vector3.zero);
        temp.y = cam.transform.localRotation.y;
        this.transform.localRotation = temp;

        this.transform.position = sheathPoint.position;
    }

    void PlaySound(AudioClip c)
    {
        if (c != null)
        {
            sheathSource.clip = c;
            sheathSource.Play();
        }

    }

    public void SpawnNewSword()
    {
        if (transform != null)
        {
            currSwordInSheath = Instantiate(swordPrefab, swordPosition.position, swordPosition.rotation, this.transform);
        }

        PlaySound(sheathSound);
    }

    public void RemoveSwordFromSheath()
    {
        if (currSwordInSheath != null)
        {
            currSwordInSheath.GetComponent<CapsuleCollider>().isTrigger = false;
            currSwordInSheath.transform.parent = null;
            currSwordInSheath = null;
            PlaySound(drawSound);
        }
    }
}
