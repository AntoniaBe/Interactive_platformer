using UnityEngine;

/// <summary>
/// Rope that holds a reference to a connected rope and can be cut with a sword.
/// </summary>
public class Rope : MonoBehaviour {

    /// <summary>
    /// Whether this rope is at the top of the rope chain.
    /// </summary>
    public bool isTopRobe;

    /// <summary>
    /// The rope connected to this rope.
    /// </summary>
    public GameObject connectedRope;

    /// <summary>
    /// Whether this rope has already been detached from its connected rope.
    /// </summary>
    public bool isDetached;

    private void OnTriggerEnter(Collider other) {
        // On collision with a sword, detach this rope and the connected rope from each other.
        if (!isDetached && other.CompareTag("sword")) {
            DetachRope();
            connectedRope.GetComponent<Rope>().DetachRope();
        }
    }

    /// <summary>
    /// Detaches this rope and enables physics behaviour on it if it isn't at the top of the rope chain.
    /// </summary>
    public void DetachRope() {
        isDetached = true;
        if (!isTopRobe) {
            transform.parent = null;
            gameObject.AddComponent<Rigidbody>();
        }
    }

}
