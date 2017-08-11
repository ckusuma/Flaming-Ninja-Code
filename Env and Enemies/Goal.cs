using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    SwordUI swordUI;
    GM gm;
    public AudioSource moneySound;

    void Start()
    {
        swordUI = Singleton_Service.GetSingleton<SwordUI>();
        gm = Singleton_Service.GetSingleton<GM>();
        moneySound = GetComponent<AudioSource>();
    }

    void Update()
    {
        //PulsateMaterial();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Left Hand") || other.CompareTag("Right Hand"))
        {
            moneySound.Play();
            swordUI.IncreaseScore(500);
            gm.CheckForWinStart(this.gameObject);
        }
    }

    public void PulsateMaterial()
    {
        MeshRenderer rendererOfObject = GetComponent<MeshRenderer>();
        Material mat;
        mat = rendererOfObject.material;

        float emission = Mathf.PingPong(Time.time, 1.0f);
        Color baseColor = Color.yellow;
        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

        mat.SetColor("_EmissionColor", finalColor);

    }
}
