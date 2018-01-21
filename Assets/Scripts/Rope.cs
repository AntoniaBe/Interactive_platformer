using UnityEngine;
using System.Collections;

public class Rope : MonoBehaviour {

    public bool isTopRobe;
    public GameObject connectedRope;
    public bool isDetached;

    private void OnTriggerEnter(Collider other) {
        if (!isDetached && other.CompareTag("sword")) {
            DetachRope();
            connectedRope.GetComponent<Rope>().DetachRope();
        }
    }

    public void DetachRope() {
        isDetached = true;
        if (!isTopRobe) {
            transform.parent = null;
            gameObject.AddComponent<Rigidbody>();
        }
    }

}
