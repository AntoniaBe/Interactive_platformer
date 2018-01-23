using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopedBoardBoard : MonoBehaviour {

    private void OnCollisionEnter(Collision collision) {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Grabbable>().isSnappedIn = false;
        GetComponent<Collider>().isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("DetectCollision");
        Destroy(this);
    }

}
