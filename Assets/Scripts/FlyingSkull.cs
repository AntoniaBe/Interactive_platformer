using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSkull : MonoBehaviour {

    public GameObject autoTarget;
    public float speed = 1f;

    private Rigidbody rigidBody;

    private void Start() {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (autoTarget) {
            var dir = (autoTarget.transform.position - transform.position).normalized;
            rigidBody.AddForce(dir * speed, ForceMode.Force);
            transform.LookAt(autoTarget.transform);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            GameController.instance.GameOver();
        }
    }

}
