using System.Collections;
using UnityEngine;

/// <summary>
/// Controller for the NPC. Makes it move to the right, react to knockbacks, jump, and die.
/// </summary>
public class NPC : MonoBehaviour {

    /// <summary>
    /// The speed at which to move the NPC.
    /// </summary>
    public float speed = 2f;

    /// <summary>
    /// The speed boost applied by the camera movement.
    /// </summary>
    public float speedBoost = 1f;

    /// <summary>
    /// The time until the NPC recovers from a knockback.
    /// </summary>
    public float knockbackRecovery = 3f;

    /// <summary>
    /// The power at which the NPC jumps.
    /// </summary>
    public float jumpSpeed = 4f;

    /// <summary>
    /// The length of the box cast for the jump check.
    /// </summary>
    public float jumpCastLength = 2.5f;

    /// <summary>
    /// The offset for the box cast of the jump check.
    /// </summary>
    public Vector3 jumpCastOffset;

    /// <summary>
    /// The size of the box cast for the jump check.
    /// </summary>
    public Vector3 jumpCastSize = new Vector3(0.1f, 0.1f, 0.1f);

    /// <summary>
    /// Whether the NPC should wait in place.
    /// </summary>
    public bool shouldWait;

    /// <summary>
    /// Whether the NPC has died (and thus no longer moves)
    /// </summary>
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

    /// <summary>
    /// Applies a knockback force to the NPC.
    /// </summary>
    /// <param name="knockback"></param>
    public void Knockback(Vector3 knockback) {
        this.knockback = knockback;
    }

    /// <summary>
    /// Kills the NPC, playing an animation and causing game over.
    /// </summary>
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

    /// <summary>
    /// Kills the NPC by falling, playing an animation and causing game over.
    /// </summary>
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