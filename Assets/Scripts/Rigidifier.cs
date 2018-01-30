using UnityEngine;
using Leap;
using Leap.Unity;

public class Rigidifier : MonoBehaviour {

    public bool withGravity = true;

    public bool CanRigidify { get; set; }

    public void Rigidify(Hand hand) {
        CanRigidify = true;

        // Create rigid body and apply hand motion to object
        var rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = false;
        rigidBody.velocity = hand.PalmVelocity.ToVector3();
        rigidBody.useGravity = withGravity;
        GetComponent<Collider>().isTrigger = false;
    }

    public void TryRigidify(Hand hand) {
        if (CanRigidify) {
            Rigidify(hand);
        }
    }

}
