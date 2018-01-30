using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSkull : MonoBehaviour {

    public GameObject autoTarget;
    public float speed = 1f;
    public bool isDead;

    private Rigidbody rigidBody;

    private void Start() {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (autoTarget && !isDead) {
            var dir = (autoTarget.transform.position - transform.position).normalized;
            rigidBody.AddForce(dir * speed, ForceMode.Force);
            transform.LookAt(autoTarget.transform);
        } else if(isDead) {
            rigidBody.AddForce(rigidBody.velocity, ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player") && !isDead) {
            GameController.instance.GameOver();
        } else if(collision.gameObject.CompareTag("hands")) {
            isDead = true;
        }
    }

}
