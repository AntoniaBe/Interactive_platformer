using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exAI : MonoBehaviour { // TODO rename to NPC or just AI or something

    public float speed = 2f;
    public float knockbackRecovery = 3f;
    public float jumpSpeed = 4f;
    public float jumpCastLength = 2.5f;
    public Vector3 jumpCastOffset;
    public Vector3 jumpCastSize = new Vector3(0.1f, 0.1f, 0.1f);
    public bool shouldWait;

    private Rigidbody rigidBody;
    private Vector3 knockback;

    private void Start() {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update() {
        // Weaken any knockback over time
        knockback = Vector3.Lerp(knockback, Vector3.zero, Time.deltaTime * knockbackRecovery);
    }

    private void FixedUpdate() {
        //Debug.DrawRay(rb.transform.position - changedY, o * rayLenght, Color.red, 1.5f);
        Debug.DrawRay(transform.position + jumpCastOffset - jumpCastSize, Vector3.right * jumpCastLength, Color.red, 1.5f);
        Debug.DrawRay(transform.position + jumpCastOffset + jumpCastSize, Vector3.right * jumpCastLength, Color.red, 1.5f);

        Vector3 targetVelocity = new Vector3(0, rigidBody.velocity.y, 0);

        if(!shouldWait) {
            // Keep walkinng to the right if not instructed to wait
            targetVelocity.x = speed;

            // Jump when about to run against a grabbable, snapped in object
            if (Physics.BoxCast(transform.position + jumpCastOffset, jumpCastSize, Vector3.right, Quaternion.identity, jumpCastLength, 1 << LayerMask.NameToLayer("SnapIn"))) {
            // if (Physics.Raycast(transform.position - changedY, to, out vision, rayLenght, 1 << LayerMask.NameToLayer("SnapIn"), QueryTriggerInteraction.Ignore)) {
                targetVelocity.y = jumpSpeed;
            }
        }

        // Apply knockback to the final velocity
        targetVelocity += knockback;

        rigidBody.velocity = targetVelocity;
    }

    public void Knockback(Vector3 knockback) {
        this.knockback = knockback;
    }

}