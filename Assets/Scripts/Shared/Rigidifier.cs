using UnityEngine;
using Leap;
using Leap.Unity;

/// <summary>
/// Provides functionality to enable physics behaviour on a Grabbable held by a Leap Motion hand.
/// </summary>
public class Rigidifier : MonoBehaviour {

    /// <summary>
    /// Whether the rigid body should be given gravity.
    /// </summary>
    public bool withGravity = true;

    /// <summary>
    /// Whether the object is allowed to have physics enabled when let go.
    /// </summary>
    public bool CanRigidify { get; set; }

    /// <summary>
    /// This will enable physics on the object, ignoring whether it's been allowed or not.
    /// </summary>
    /// <param name="hand">the hand that was holding the Grabbable</param>
    public void Rigidify(Hand hand) {
        CanRigidify = true;

        // Create rigid body and apply hand motion to object
        var rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = false;
        rigidBody.velocity = hand.PalmVelocity.ToVector3();
        rigidBody.useGravity = withGravity;
        GetComponent<Collider>().isTrigger = false;
    }

    /// <summary>
    /// If allowed, this will enable physics on the object. Should be called when the Grabbable is let go.
    /// </summary>
    /// <param name="hand">the hand that was holding the Grabbable</param>
    public void TryRigidify(Hand hand) {
        if (CanRigidify) {
            Rigidify(hand);
        }
    }

}
