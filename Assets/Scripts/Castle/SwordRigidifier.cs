using UnityEngine;
using Leap;
using Leap.Unity;

public class SwordRigidifier : MonoBehaviour {

    public float velocityScale = 1f;
    public Rope[] ropes;

    public void SwordUngrabbed(Hand hand) {
        // Sword only gets physics once all ropes are cut
        foreach (var rope in ropes) {
            if (!rope.isDetached) {
                return;
            }
        }

        // Create rigid body and apply hand motion to sword
        var rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = false;
        rigidBody.velocity = hand.PalmVelocity.ToVector3();
        GetComponent<Collider>().isTrigger = false;
    }

}
