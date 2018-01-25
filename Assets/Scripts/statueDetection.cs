using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statueDetection : MonoBehaviour {

    public Transform target;
    public Transform trigger;
    public bool status = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(target.position == trigger.position)
        {
            status = true;
        }
	}
}
