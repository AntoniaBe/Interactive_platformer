using UnityEngine;

/// <summary>
/// Projectile that flies towards the player and causes a knockback on collision.
/// </summary>
public class DartProjectile : MonoBehaviour {

    /// <summary>
    /// The player object to target.
    /// </summary>
    public GameObject autoTarget;

    /// <summary>
    /// The speed at which the projectile flies.
    /// </summary>
    public float speed = 1f;

    /// <summary>
    /// The time until the projectile disappears.
    /// </summary>
    public float lifeTime = 2f;

    /// <summary>
    /// The amount of knockback applied on collision with the player.
    /// </summary>
    public float knockbackStrength = 1f;

    private Rigidbody rigidBody;

    private void Start() {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update() {
        // If a target has been set, go towards it, otherwise just fly left
        if (autoTarget == null || transform.position.x <= autoTarget.transform.position.x) {
            rigidBody.velocity = Vector3.left * speed;
        } else {
            rigidBody.velocity = (autoTarget.transform.position - transform.position).normalized * speed;
        }

        // Kill this object once its lifetime has run out
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        var ai = collision.gameObject.GetComponent<NPC>();
        if (ai) {
            ai.Knockback(Vector3.left * knockbackStrength);
        }
        Destroy(gameObject);
    }

}
