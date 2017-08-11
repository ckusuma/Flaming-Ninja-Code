using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheathPosition : MonoBehaviour {

    public Transform camera;
	
	// Update is called once per frame
	void Update () {
        this.transform.position = camera.transform.position;
        
	}
}
