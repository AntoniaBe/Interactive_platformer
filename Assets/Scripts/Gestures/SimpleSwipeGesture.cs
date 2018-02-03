using UnityEngine;
using Leap;
using Leap.Unity;

/// <summary>
/// Leap Gesture detecting a swipe of the hand. Very simple, will conflict with other gestures - therefore only used on victory and game over screen.
/// </summary>
public class SimpleSwipeGesture : MonoBehaviour {

    /// <summary>
    /// Event fired when a swipe has been detected.
    /// </summary>
    public BooleanEvent onSwipeEvent;

    /// <summary>
    /// The minimum velocity for it to be counted as a swipe.
    /// </summary>
    public float minVelocity = 3f;

    /// <summary>
    /// The cooldown between swipe events.
    /// </summary>
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
        // Don't bother checking if we're on a cooldown period
        if (cooldownTimer > 0f) {
            cooldownTimer -= Time.unscaledDeltaTime;
            return;
        }

        Frame frame = leapServiceProvider.CurrentFrame;
        foreach (var hand in frame.Hands) {
            // Only proceed if the hand is actually in swiping position
            if (!IsHandSwipable(hand)) {
                continue;
            }

            // Invoke a swipe to the left or right depending on the velocity
            if (hand.PalmVelocity.x > minVelocity) {
                onSwipeEvent.Invoke(true);
                cooldownTimer = cooldownTime;
            } else if (hand.PalmVelocity.x < -minVelocity) {
                onSwipeEvent.Invoke(false);
                cooldownTimer = cooldownTime;
            }
        }
    }

    /// <summary>
    /// Check if the hand is a candidate for swiping - the normal must be pointing to the left or right and the middle three fingers must be extended.
    /// </summary>
    /// <param name="hand">the hand to be checked</param>
    /// <returns>true if the hand is a candidate for swiping</returns>
    private bool IsHandSwipable(Hand hand) {
        // Check if the palm is pointing to the left or right
        if (Mathf.Abs(hand.PalmNormal.x) < 0.5f) {
            return false;
        }

        // Make sure all relevant fingers are extended
        var relevantFingers = new Finger.FingerType[] { Finger.FingerType.TYPE_INDEX, Finger.FingerType.TYPE_MIDDLE, Finger.FingerType.TYPE_RING };
        foreach (var fingerType in relevantFingers) {
            if (!hand.Fingers[(int)fingerType].IsExtended) {
                return false;
            }
        }

        return true;
    }

}