using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {

    SwordUI swordUI;
    float damage = 5;
	// Use this for initialization
	void Start () {
        swordUI = Singleton_Service.GetSingleton<SwordUI>();
	}

    void OnCollisionEnter(Collision other)
    {
        Destroy(this.gameObject);
    }
}
