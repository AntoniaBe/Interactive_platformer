using UnityEngine;

/// <summary>
/// Takes care of detaching the roped board once all ropes have been cut.
/// </summary>
public class RopedBoard : MonoBehaviour {

    /// <summary>
    /// The board object attached in this roped board.
    /// </summary>
    public GameObject board;

    /// <summary>
    /// The sword to be used for cutting the ropes.
    /// </summary>
    public Rigidifier sword;

    private Rope[] ropes;
    private bool isBoardDetached;

    private void Start() {
        ropes = GetComponentsInChildren<Rope>();
    }

    private void Update() {
        if (!isBoardDetached) {
            // Go through all ropes and check if they've been cut
            var allRopesDetached = true;
            foreach (var rope in ropes) {
                if (!rope.isDetached) {
                    allRopesDetached = false;
                    break;
                }
            }

            // Only detach the board if all ropes have been cut
            if (allRopesDetached) {
                isBoardDetached = true;

                sword.CanRigidify = true;

                var rb = board.AddComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX;
                board.transform.parent = null;
            }
        }
    }

}
