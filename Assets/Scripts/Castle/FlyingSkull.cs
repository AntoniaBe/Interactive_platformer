using UnityEngine;

/// <summary>
/// Flying enemy skull that follows and kills the player on touch. Can be killed by slapping it with a hand.
/// </summary>
public class FlyingSkull : MonoBehaviour {

    /// <summary>
    /// The player object to follow.
    /// </summary>
    public GameObject autoTarget;

    /// <summary>
    /// The speed at which to fly.
    /// </summary>
    public float speed = 1f;

    /// <summary>
    /// Whether this flying skull has already been killed.
    /// </summary>
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
        } else if (isDead) {
            rigidBody.AddForce(rigidBody.velocity, ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player") && !isDead) {
            GameController.instance.GameOver();
        } else if (collision.gameObject.CompareTag("hands")) {
            isDead = true;
        }
    }

}
