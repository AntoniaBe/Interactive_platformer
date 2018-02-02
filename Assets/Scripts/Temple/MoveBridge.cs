using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBridge : MonoBehaviour {

    public GameObject statueLarge;
    public GameObject statueSmall;
    public GameObject bridgeTrigger;

	/// <summary>
    /// Get "status" information from all statues and check whether they are true. If all are true, move bridge upwards
    /// </summary>
	void LateUpdate () {
        if (statueLarge.GetComponent<StatueDetection>().status && statueSmall.GetComponent<StatueDetection>().status) {
            if(transform.position.y < bridgeTrigger.transform.position.y)
            {
                transform.Translate(Vector3.up * Time.deltaTime, Space.World);
            }
        }
	}
}
