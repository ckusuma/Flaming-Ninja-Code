using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOrientation : MonoBehaviour {

    public Transform playerPos;
	
	void Update () {
        this.transform.LookAt(playerPos);
	}
}
