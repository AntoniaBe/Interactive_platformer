using System.Linq;
using UnityEngine;
using Leap;
using Leap.Unity;

public class GrabGesture : MonoBehaviour {

    public float minGrabStrength = 0.75f;
    public int minUnextendedFingers = 3;
    public bool allowRotation = true;

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
            var grabbing = hand.GrabStrength > minGrabStrength || IsGrabbingDespiteGrabStrength(hand);
            if (grabbing) {
                if (!currentGrabbables[handIndex]) {
                    currentGrabbables[handIndex] = grabbables.FirstOrDefault(t => DoesHandMatch(t.touchingHand, hand) && !t.isSnappedIn);
                }

                if (currentGrabbables[handIndex]) {
                    if (currentGrabbables[handIndex].isSnappedIn) {
                        currentGrabbables[handIndex] = null;
                    } else {
                        currentGrabbables[handIndex].transform.position = hand.PalmPosition.ToVector3();
                        if (allowRotation) {
                            currentGrabbables[handIndex].transform.rotation = hand.Rotation.ToQuaternion();
                        }
                    }
                }
            } else {
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
