using System.Linq;
using UnityEngine;
using Leap;
using Leap.Unity;

/// <summary>
/// Leap Gesture detecting whether a Grabbable object is being grabbed and moved around.
/// </summary>
public class GrabGesture : MonoBehaviour {

    /// <summary>
    /// The minimum amount of grab strength for a grab to be continued. Initial grabs always require full grab strength.
    /// </summary>
    public float minGrabStrength = 0.75f;

    /// <summary>
    /// The minimum amount of unextended fingers for a grab to be continued. This can act in place of the grab strength.
    /// </summary>
    public int minUnextendedFingers = 3;

    private LeapServiceProvider leapServiceProvider;
    private Grabbable[] grabbables;

    private Grabbable[] currentGrabbables;

    private void Start() {
        leapServiceProvider = FindObjectOfType<LeapServiceProvider>();

        currentGrabbables = new Grabbable[2]; // one for each hand

        grabbables = FindObjectsOfType<Grabbable>();
    }

    private void Update() {
        var frame = leapServiceProvider.CurrentFrame;
        foreach (var hand in frame.Hands) {
            var handIndex = hand.IsRight ? 0 : 1;
            // The initial grab needs to be harder than followup checks to prevent accidental grabs
            var grabbing = hand.GrabStrength >= 1f;

            // If the player is already holding something, don't be as harsh, to prevent letting go due to Leap errors
            if (currentGrabbables[handIndex]) {
                grabbing = hand.GrabStrength >= minGrabStrength || IsGrabbingDespiteGrabStrength(hand);
            }

            // If we're grabbing something with this hand, move it with the hand and fire appropriate events
            if (grabbing) {
                // If nothing has been grabbed yet, find a Grabbable candidate and fire the start grabbing event
                if (!currentGrabbables[handIndex]) {
                    currentGrabbables[handIndex] = grabbables.FirstOrDefault(t => DoesHandMatch(t.TouchingHand, hand) && !t.IsSnappedIn);
                    if (currentGrabbables[handIndex]) {
                        currentGrabbables[handIndex].StartGrabbing(hand);
                    }
                }

                // If we've already been grabbing a Grabbable or just started, move and rotate it with the hand
                if (currentGrabbables[handIndex]) {
                    // If the object has since snapped in, let go of it
                    if (currentGrabbables[handIndex].IsSnappedIn) {
                        currentGrabbables[handIndex].StopGrabbing(hand);
                        currentGrabbables[handIndex] = null;
                    } else {
                        // Only move with hand if the Grabbable allows movement
                        if (currentGrabbables[handIndex].allowMovement) {
                            currentGrabbables[handIndex].transform.position = hand.PalmPosition.ToVector3();
                        }

                        // Only rotate with hand if the Grabbable allows rotation
                        if (currentGrabbables[handIndex].allowRotation) {
                            currentGrabbables[handIndex].transform.rotation = hand.Rotation.ToQuaternion();
                        }

                        // Fire the GrabUpdate event on every frame to allow custom handling within the Grabbable
                        currentGrabbables[handIndex].GrabUpdate(hand);
                    }
                }
            } else {
                // If we're no longer grabbing something but we still have something attached to the hand, let go of it
                if (currentGrabbables[handIndex]) {
                    currentGrabbables[handIndex].StopGrabbing(hand);
                }
                currentGrabbables[handIndex] = null;
            }
        }
    }

    /// <summary>
    /// Checks if the physical hand object belongs to the given Leap hand.
    /// </summary>
    /// <param name="handObject">the physical hand object</param>
    /// <param name="hand">the Leap representation of the hand</param>
    /// <returns>true if the physical hand belongs to the Leap hand</returns>
    private bool DoesHandMatch(GameObject handObject, Hand hand) {
        if (!handObject) {
            return false;
        }

        var rigidHand = handObject.GetComponentInParent<RigidHand>();
        return rigidHand && (rigidHand.Handedness == Chirality.Left) == hand.IsLeft;
    }

    /// <summary>
    /// Check if fingers are at least mostly not-extended. Leap Motion's GrabStrength fails because the index finger randomly extends.
    /// </summary>
    /// <param name="hand">the hand to be checked</param>
    /// <returns>true if there's enough unextended fingers for it to be considered a grab</returns>
    private bool IsGrabbingDespiteGrabStrength(Hand hand) {
        var notExtendedCount = 0;
        foreach (var finger in hand.Fingers) {
            if (!finger.IsExtended) {
                notExtendedCount++;
            }
        }
        return notExtendedCount >= minUnextendedFingers;
    }

}
