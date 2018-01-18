using UnityEngine;
using Leap;
using Leap.Unity;

public class PointingGesture : MonoBehaviour {

    public float minVelocity = 1f;
    public float velocitySwitchTime = 1f;
    public int minVelocitySwitches = 2;

    private LeapServiceProvider leapServiceProvider;
    private float lastPointTime = -1f;
    private float initialPointVelocityX;
    private float lastPointVelocityX;
    private int velocitySwitches;

    private void Start() {
        leapServiceProvider = FindObjectOfType<LeapServiceProvider>();
    }

    private void Update() {
        Frame frame = leapServiceProvider.CurrentFrame;
        foreach (var hand in frame.Hands) {
            if (!IsPointingFinger(hand)) {
                continue;
            }

            var finger = hand.Fingers[(int) Finger.FingerType.TYPE_INDEX];
            if (finger.TipVelocity.x > -minVelocity && finger.TipVelocity.x < minVelocity) {
                continue;
            }

            if (lastPointTime != -1f && lastPointTime < Time.unscaledTime + velocitySwitchTime) {
                var lastSign = Mathf.Sign(lastPointVelocityX);
                var sign = Mathf.Sign(finger.TipVelocity.x);
                if (lastSign != sign) {
                    velocitySwitches++;
                    if (velocitySwitches > minVelocitySwitches) {
                        OnPointing(initialPointVelocityX);
                    }
                }
            } else {
                velocitySwitches = 0;
                initialPointVelocityX = finger.TipVelocity.x;
            }
            lastPointTime = Time.unscaledTime;
            lastPointVelocityX = finger.TipVelocity.x;
        }
    }

    private bool IsPointingFinger(Hand hand) {
        // Index finger must always be extended for this gesture.
        if (!hand.Fingers[(int) Finger.FingerType.TYPE_INDEX].IsExtended) {
            return false;
        }

        // At least middle and ring finger must not be extended. Not checking for pinky because Leap Motion isn't very reliable...
        var otherFingers = new Finger.FingerType[] { Finger.FingerType.TYPE_MIDDLE, Finger.FingerType.TYPE_RING };
        var foundNotExtended = false;
        foreach (var fingerType in otherFingers) {
            if (!hand.Fingers[(int) fingerType].IsExtended) {
                foundNotExtended = true;
                break;
            }
        }

        return foundNotExtended;
    }

    private void OnPointing(float velocityX) {
        print("pointing to " + velocityX);
    }

}