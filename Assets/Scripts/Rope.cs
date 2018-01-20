using UnityEngine;
using System.Collections;

public class Rope : MonoBehaviour {

    public bool isTopRobe;
    public GameObject connectedRope;

    private void OnTriggerEnter(Collider other) {
        DetachRope();
        connectedRope.GetComponent<Rope>().DetachRope();
    }

    public void DetachRope() {
        if (!isTopRobe) {
            transform.parent = null;
            gameObject.AddComponent<Rigidbody>();
        }
    }

}
