using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public float speed = 2f;
    public float speedBoost = 1f;
    public float knockbackRecovery = 3f;
    public float jumpSpeed = 4f;
    public float jumpCastLength = 2.5f;
    public Vector3 jumpCastOffset;
    public Vector3 jumpCastSize = new Vector3(0.1f, 0.1f, 0.1f);
    public bool shouldWait;
    public bool isDead;

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
        Vector3 targetVelocity = new Vector3(0, rigidBody.velocity.y, 0);

        if (!shouldWait && !isDead) {
            // Keep walkinng to the right if not instructed to wait
            targetVelocity.x = speed * speedBoost;

            // Jump when about to run against a grabbable, snapped in object
            if (Physics.BoxCast(transform.position + jumpCastOffset, jumpCastSize, Vector3.right, Quaternion.identity, jumpCastLength, 1 << LayerMask.NameToLayer("SnapIn"))) {
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

    public void Die() {
        GetComponent<Animation>().Stop();
        isDead = true;
        StartCoroutine(DeathAnimation());
        GameController.instance.GameOver();
    }

    private IEnumerator DeathAnimation() {
        const float deathTime = 0.5f;
        float timer = 0f;
        float angle = 0f;
        while (timer < deathTime) {
            angle = Mathf.LerpAngle(angle, -91f, timer / deathTime);
            transform.localEulerAngles = new Vector3(-9.75f, 97f, angle);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void FallToDeath() {
        isDead = true;
        StartCoroutine(FallingAnimation());
        GameController.instance.GameOver();
    }

    private IEnumerator FallingAnimation() {
        while (true) {
            transform.Rotate(new Vector3(Random.value, Random.value, Random.value));
            yield return null;
        }
    }

}