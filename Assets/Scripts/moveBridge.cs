using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBridge : MonoBehaviour {

    public GameObject statueLarge;
    public GameObject statueSmall;
    public GameObject statueTiny;
    public GameObject bridgeTrigger;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

        if (statueLarge.GetComponent<statueDetection>().status && statueSmall.GetComponent<statueDetection>().status && statueTiny.GetComponent<statueDetection>().status) {
            if(transform.position.y < bridgeTrigger.transform.position.y)
            {
                Debug.Log(transform.position.y);
                transform.Translate(Vector3.up * Time.deltaTime, Space.World);
            }
        }
	}
}
