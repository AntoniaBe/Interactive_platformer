using UnityEngine;
using UnityEngine.Events;
using Leap;
using Leap.Unity;

/// <summary>
/// Leap Gesture detecting a clapping movement with both hands.
/// </summary>
public class ClapGesture : MonoBehaviour {

    /// <summary>
    /// Event fired when a clap has been detected.
    /// </summary>
    public UnityEvent onClapEvent;

    /// <summary>
    /// The distance at which hands are considered close enough to each other to count as a clap should one of the hands lose tracking due to the hands touching.
    /// </summary>
    public float nearDistance = 3f;

    /// <summary>
    /// The distance at which hands are considered close enough to count as a clap no matter what.
    /// </summary>
    public float clapDistance = 2f;

    /// <summary>
    /// The grace period before a movement is no longer considered a clap, even if tracking was lost.
    /// </summary>
    public float probablyClappingCooldown = 0.5f;

    /// <summary>
    /// The cooldown applied between clap events.
    /// </summary>
    public float clappingCooldown = 1f;

    /// <summary>
    /// The maximum angle the hands can be from each other for it to count as a clap.
    /// </summary>
    public float maxAngle = 55f;

    /// <summary>
    /// The minimum velocity hands must move at for it to count as a clap.
    /// </summary>
    public float minVelocity = 0.5f;

    private LeapServiceProvider leapServiceProvider;
    private float lastClapTime;
    private float probablyClappingTime = -1f;

    private void Awake() {
        if (onClapEvent == null) {
            onClapEvent = new UnityEvent();
        }
    }

    private void Start() {
        leapServiceProvider = FindObjectOfType<LeapServiceProvider>();
    }

    private void Update() {
        // Don't bother doing checks if we're not allowed to clap
        if (!CanClap()) {
            return;
        }

        // You need two hands to clap - if one hand disappeared while we were probably clapping, the clap most likely occurred.
        // This is because the Leap Motion usually loses one of the hands once they touch.
        if (!AreBothHandsTracked()) {
            if (Time.unscaledTime - probablyClappingTime < probablyClappingCooldown) {
                Clap();
                lastClapTime = Time.unscaledTime;
                probablyClappingTime = -1f;
            }
            return;
        }

        // Check gesture stuff to see if we're probably, or even actually, clapping.
        float handDistance = GetHandsDistance();
        if (IsProbablyClapping(handDistance)) {
            probablyClappingTime = Time.unscaledTime;

            if (handDistance < clapDistance) {
                lastClapTime = Time.unscaledTime;
                Clap();
                probablyClappingTime = -1f;
            }
        }
    }

    /// <summary>
    /// Checks whether both hands are currently being tracked.
    /// </summary>
    /// <returns>true if both hands are tracked</returns>
    private bool AreBothHandsTracked() {
        Frame frame = leapServiceProvider.CurrentFrame;
        return frame.Hands.Count >= 2;
    }

    /// <summary>
    /// Checks if the gesture is allowed to fire a clap event.
    /// </summary>
    /// <returns></returns>
    private bool CanClap() {
        // Cooldown to prevent spam-clapping
        if (Time.unscaledTime - lastClapTime < clappingCooldown) {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if the player is probably clapping. This means the clap movement has started, but may not be completed yet. Used to activate a grace period in case a hand loses tracking during the clap.
    /// </summary>
    /// <param name="handDistance">the distance between both hands</param>
    /// <returns>true if the player is probably clapping</returns>
    private bool IsProbablyClapping(float handDistance) {
        Frame frame = leapServiceProvider.CurrentFrame;
        foreach (Hand hand in frame.Hands) {
            // At least the index and middle finger of both hands must be extended
            if (!hand.Fingers[(int)Finger.FingerType.TYPE_INDEX].IsExtended || !hand.Fingers[(int)Finger.FingerType.TYPE_MIDDLE].IsExtended) {
                return false;
            }

            // Hand must be facing the other hand
            float angle = Vector3.Angle(hand.PalmNormal.ToVector3(), hand.IsLeft ? Vector3.right : Vector3.left);
            if (angle > maxAngle) {
                return false;
            }

            // Hand must be moving towards the other hand
            if (hand.IsLeft && hand.PalmVelocity.x < minVelocity) {
                return false;
            } else if (!hand.IsLeft && hand.PalmVelocity.x > minVelocity) {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Returns the distance between both hands.
    /// </summary>
    /// <returns>the distance between both hands</returns>
    private float GetHandsDistance() {
        Vector3 leftHandPosition = new Vector3();
        Vector3 rightHandPosition = new Vector3();

        Frame frame = leapServiceProvider.CurrentFrame;
        foreach (Hand hand in frame.Hands) {
            if (hand.IsLeft) {
                leftHandPosition = hand.PalmPosition.ToVector3();
            } else {
                rightHandPosition = hand.PalmPosition.ToVector3();
            }
        }

        return Vector3.Distance(leftHandPosition, rightHandPosition);
    }

    /// <summary>
    /// Invokes the clap event.
    /// </summary>
    private void Clap() {
        onClapEvent.Invoke();
    }

}
