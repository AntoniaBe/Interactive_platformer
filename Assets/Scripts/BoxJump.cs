using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxJump : MonoBehaviour { // TODO this shouldn't be necessary anymore since it will jump on SnapIn layer now instead of jump tag

    public GameObject target;
    
	/// <summary>
    /// Check if target object is in correct position, then set the objects tag to "jump"
    /// </summary>
	void Update () {
        if (target.GetComponent<Grabbable>().IsSnappedIn) {
            gameObject.tag = "jump";
        }
    }
}
