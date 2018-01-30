using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxJump : MonoBehaviour {

    public GameObject target;
    
	/// <summary>
    /// Check if target object is in correct position, then set the objects tag to "jump"
    /// </summary>
	void Update () {
        if (target.GetComponent<Grabbable>().isSnappedIn) {
            gameObject.tag = "jump";
        }
    }
}
