using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowNPC : MonoBehaviour {

    public Transform target;
    public Vector3 offset = new Vector3(0f, 1.0f, -2.0f);

    // Use this for initialization
    void Start () {
		
	}

    void LateUpdate() {
        transform.position = target.position + offset;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
