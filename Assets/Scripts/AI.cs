using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {

    private Rigidbody rb;
    public float speed = 0.4f;

	void Start () {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // rb.AddForce(new Vector3(speed, 0, 0));
        rb.velocity = new Vector3(speed, 0, 0);
    }
}
