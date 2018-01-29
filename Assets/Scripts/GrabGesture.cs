using System.Linq;
using UnityEngine;
using Leap;
using Leap.Unity;

public class GrabGesture : MonoBehaviour {

    public float minGrabStrength = 0.75f;
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
                grabbing = hand.GrabStrength > minGrabStrength || IsGrabbingDespiteGrabStrength(hand);
            }
            if (grabbing) {
                if (!currentGrabbables[handIndex]) {
                    currentGrabbables[handIndex] = grabbables.FirstOrDefault(t => DoesHandMatch(t.touchingHand, hand) && !t.isSnappedIn);
                    if (currentGrabbables[handIndex]) {
                        currentGrabbables[handIndex].StartGrabbing(hand);
                    }
                }

                if (currentGrabbables[handIndex]) {
                    if (currentGrabbables[handIndex].isSnappedIn) {
                        currentGrabbables[handIndex].StopGrabbing(hand);
                        currentGrabbables[handIndex] = null;
                    } else { 
                        if (currentGrabbables[handIndex].allowMovement) {
                            currentGrabbables[handIndex].transform.position = hand.PalmPosition.ToVector3();
                        }

                        if (currentGrabbables[handIndex].allowRotation) {
                            currentGrabbables[handIndex].transform.rotation = hand.Rotation.ToQuaternion();
                        }

                        currentGrabbables[handIndex].GrabUpdate(hand);
                    }
                }
            } else {
                if (currentGrabbables[handIndex]) {
                    currentGrabbables[handIndex].StopGrabbing(hand);
                }
                currentGrabbables[handIndex] = null;
            }
        }
    }

    private bool DoesHandMatch(GameObject handObject, Hand hand) {
        if (!handObject) {
            return false;
        }

        return (handObject.GetComponentInParent<RigidHand>().Handedness == Chirality.Left) == hand.IsLeft;
    }

    /// <summary>
    /// Check if fingers are at least mostly not-extended. Leap Motion's GrabStrength fails because the index finger randomly extends.
    /// </summary>
    /// <param name="hand"></param>
    /// <returns></returns>
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
