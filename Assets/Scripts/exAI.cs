using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exAI : MonoBehaviour { // TODO rename to NPC or just AI or something

    private Rigidbody rb;
    public float speed = 2f;
    public float knockbackRecovery = 3f;
    public float rayLenght = 3f;
    private float jumpSpeed = 4f;
    private RaycastHit vision;
    private Vector3 to = new Vector3(1f, 0f, 0f);
    public Vector3 changedY = new Vector3(0f, -2f, 0f);
    public bool shouldWait;

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

        Vector3 targetVelocity = new Vector3(0, rb.velocity.y, 0);

        if(!shouldWait) {
            // Keep walkinng to the right if not instructed to wait
            targetVelocity.x = speed;

            // Jump when about to run against a grabbable, snapped in object
            if (Physics.Raycast(transform.position - changedY, to, out vision, rayLenght, 1 << LayerMask.NameToLayer("SnapIn"), QueryTriggerInteraction.Ignore)) {
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