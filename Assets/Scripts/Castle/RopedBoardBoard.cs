using UnityEngine;

/// <summary>
/// Makes the roped board grabbable once it collides with anything (e.g. the ground).
/// </summary>
public class RopedBoardBoard : MonoBehaviour {

    private void OnCollisionEnter(Collision collision) {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Grabbable>().IsSnappedIn = false;
        GetComponent<Collider>().isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("DetectCollision");
        Destroy(this);
    }

}
