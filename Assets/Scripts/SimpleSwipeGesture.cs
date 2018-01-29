using UnityEngine;
using Leap;
using Leap.Unity;

public class SimpleSwipeGesture : MonoBehaviour {

    public BooleanEvent onSwipeEvent;

    public float minVelocity = 3f;
    public float cooldownTime = 1f;

    private LeapServiceProvider leapServiceProvider;
    private float cooldownTimer;

    private void Awake() {
        if (onSwipeEvent == null) {
            onSwipeEvent = new BooleanEvent();
        }
    }

    private void Start() {
        leapServiceProvider = FindObjectOfType<LeapServiceProvider>();
    }

    private void Update() {
        if (cooldownTimer > 0f) {
            cooldownTimer -= Time.unscaledDeltaTime;
            return;
        }

        Frame frame = leapServiceProvider.CurrentFrame;
        foreach (var hand in frame.Hands) {
            if (!IsHandSwipable(hand)) {
                continue;
            }

            if (hand.PalmVelocity.x > minVelocity) {
                onSwipeEvent.Invoke(true);
                cooldownTimer = cooldownTime;
            } else if (hand.PalmVelocity.x < -minVelocity) {
                onSwipeEvent.Invoke(false);
                cooldownTimer = cooldownTime;
            }
        }
    }

    private bool IsHandSwipable(Hand hand) {
        if (Mathf.Abs(hand.PalmNormal.x) < 0.5f) {
            return false;
        }

        // Make sure all relevant fingers are extended
        var relevantFingers = new Finger.FingerType[] { Finger.FingerType.TYPE_INDEX, Finger.FingerType.TYPE_MIDDLE, Finger.FingerType.TYPE_RING };
        foreach (var fingerType in relevantFingers) {
            if (!hand.Fingers[(int) fingerType].IsExtended) {
                return false;
            }
        }

        return true;
    }

}