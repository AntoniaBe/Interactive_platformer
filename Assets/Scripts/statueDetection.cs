using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueDetection : MonoBehaviour {

    public Transform target;
    public Transform trigger;
    public bool status = false;
    
	void Start () {
		
	}
	/// <summary>
    /// Check if spezific statue is in the correct place, if so "status" is true 
    /// </summary>
	void LateUpdate () {
		if(target.position == trigger.position)
        {
            status = true;
        }
	}
}
