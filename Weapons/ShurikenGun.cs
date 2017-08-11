using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenGun : MonoBehaviour {

    //Singletons
    Input_Listeners IPL;
    SwordUI swordUI;

    [SerializeField]
    float reloadTime;
    [SerializeField]
    float fireRate;
    [SerializeField]
    float spinSpeed;
    [SerializeField]
    float shootingRange;

    float usageCost = 1;

    int ammo = 0;
    int maxAmmo = 20;

    [SerializeField]
    GameObject shurikenPrefab;
    [SerializeField]
    Transform shurikenLocation;
    [SerializeField]
    Transform refDirection;
    [SerializeField]
    Left_VR_Cont leftController;
    [SerializeField]
    Right_VR_Cont rightController;

    AudioSource audio;
    public AudioClip shootingNoise;

    GameObject shurikenLoaded;

    bool reloadInProgress;
    bool leftShootingStarted;

    bool inLeftHand;

    public Transform directionRef1;
    public Transform directionRef2;

    void Start()
    {
        IPL = Singleton_Service.GetSingleton<Input_Listeners>();
        swordUI = Singleton_Service.GetSingleton<SwordUI>();
        audio = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        if (swordUI.FireIsDepleted())
        {
            return;
        }
        if ((leftController.controller.GetPressDown(leftController.triggerButton) /*&& !leftShootingStarted*/))
        {
            leftShootingStarted = true;
            StartCoroutine("ShootLeftGun");
        }
    }

    public void PlayGunNoise(AudioClip clip)
    {
        if (clip != null && audio != null)
        {
            audio.clip = clip;
            audio.Play();
        }

    }

    IEnumerator ShootLeftGun()
    {
        InstantiateNewShuriken();
        AddForceAndTorqueToEquippedShuriken();
        leftController.PulseVibrate(10, 10, 0.01f, 1);
        swordUI.DepleteFire(usageCost);
        PlayGunNoise(shootingNoise);
        yield return new WaitForSeconds(0.1f);
        InstantiateNewShuriken();
        AddForceAndTorqueToEquippedShuriken();
        leftController.PulseVibrate(10, 10, 0.01f, 1);
        swordUI.DepleteFire(usageCost);
        PlayGunNoise(shootingNoise);
        yield return new WaitForSeconds(0.1f);
        InstantiateNewShuriken();
        AddForceAndTorqueToEquippedShuriken();
        leftController.PulseVibrate(10, 10, 0.01f, 1);
        swordUI.DepleteFire(usageCost);
        PlayGunNoise(shootingNoise);
        yield return new WaitForSeconds(0.5f);
        leftShootingStarted = false;
    }

    void InstantiateNewShuriken()
    {
        if (shurikenLoaded == null)
        {
            shurikenLoaded = Instantiate(shurikenPrefab, shurikenLocation.position, shurikenLocation.rotation);
        }
        else
        {
            shurikenLoaded.transform.parent = null;
            shurikenLoaded = Instantiate(shurikenPrefab, shurikenLocation.position, shurikenLocation.rotation);
        }
    }

    void AddForceAndTorqueToEquippedShuriken()
    {
        if (shurikenLoaded != null)
        {
            Rigidbody rb = shurikenLoaded.GetComponent<Rigidbody>();
            GunShuriken gunShuriken = shurikenLoaded.GetComponent<GunShuriken>();
            rb.AddForce((directionRef2.position - directionRef1.position).normalized * shootingRange, ForceMode.Force);
            rb.AddRelativeTorque(Vector3.Cross(gunShuriken.ref1.localPosition, gunShuriken.ref2.localPosition) * spinSpeed);
            shurikenLoaded.transform.parent = null;
        }
    }



}
