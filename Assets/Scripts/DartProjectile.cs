using UnityEngine;
using System.Collections;

public class DartProjectile : MonoBehaviour {

    public GameObject autoTarget;
    public float speed = 1f;
    public float lifeTime = 2f;
    public float knockbackStrength = 1f;

    private Rigidbody rigidBody;

    private void Start() {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update() {
        if(autoTarget == null || transform.position.x <= autoTarget.transform.position.x) {
            rigidBody.velocity = Vector3.left * speed;
        } else {
            rigidBody.velocity = (autoTarget.transform.position - transform.position).normalized * speed;
        }
        

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        var ai = collision.gameObject.GetComponent<exAI>();
        if (ai) {
            ai.Knockback(Vector3.left * knockbackStrength);
        }
        Destroy(gameObject);
    }

}
