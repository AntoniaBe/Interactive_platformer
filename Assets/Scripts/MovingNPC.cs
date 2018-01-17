using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingNPC : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        Vector3 current = transform.position;
        float movespeed = 0.05f;
        transform.position = new Vector3(transform.position.x + movespeed, current.y);
    }
}
