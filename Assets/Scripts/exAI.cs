using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exAI : MonoBehaviour {

    private Rigidbody rb;
    public float speed = 2f;
    public float knockbackRecovery = 3f;
    public float rayLenght = 3f;
    private float jumpSpeed = 4f;
    private RaycastHit vision;
    private Vector3 to = new Vector3(1f, 0f, 0f);
    public Vector3 changedY = new Vector3(0f, -2f, 0f);

    private Vector3 knockback;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        // Weaken any knockback over time
        knockback = Vector3.Lerp(knockback, Vector3.zero, Time.deltaTime * knockbackRecovery);
    }


    private void FixedUpdate() {
        Debug.DrawRay(rb.transform.position - changedY, to * rayLenght, Color.red, 1.5f);

        Vector3 targetVelocity = new Vector3(speed, rb.velocity.y, 0);

        // Jump when about to run against a grabbable object
        if (Physics.Raycast(transform.position - changedY, to, out vision, rayLenght)) {
            if (vision.collider.CompareTag("grab")) {
                targetVelocity.y = jumpSpeed;
            }
        }

        // Apply knockback to the final velocity
        targetVelocity += knockback;


        rb.velocity = targetVelocity;
    }

    public void Knockback(Vector3 knockback) {
        this.knockback = knockback;
    }

}