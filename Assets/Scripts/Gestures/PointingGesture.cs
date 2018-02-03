using UnityEngine;
using Leap;
using Leap.Unity;

/// <summary>
/// Leap Gesture detecting a finger pointing repeatedly in a direction (left or right).
/// </summary>
public class PointingGesture : MonoBehaviour {

    /// <summary>
    /// Event fired when a pointing finger has been detected.
    /// </summary>
    public BooleanEvent onPointEvent;

    /// <summary>
    /// The mininum velocity of the finger for it to be counted as a pointing movement.
    /// </summary>
    public float minVelocity = 1f;

    /// <summary>
    /// The maximum amounf of time allowed between switches of velocity direction.
    /// </summary>
    public float velocitySwitchTime = 1f;

    /// <summary>
    /// The amount of velocity switches (going back and forth) required for it to be counted as a pointing gesture.
    /// </summary>
    public int minVelocitySwitches = 2;

    /// <summary>
    /// The cooldown between pointing events.
    /// </summary>
    public float cooldownTime = 1f;

    /// <summary>
    /// Whether to use the palm velocity instead of the finger velocity.
    /// </summary>
    public bool usePalmVelocity = true;

    private LeapServiceProvider leapServiceProvider;
    private float lastPointTime = -1f;
    private float lastPointVelocityX;
    private int lastPointingDir;
    private int velocitySwitches;
    private float cooldownTimer;

    private void Awake() {
        if (onPointEvent == null) {
            onPointEvent = new BooleanEvent();
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
            // Make sure the hand is actually pointing to the left or right
            if (!IsPointingFinger(hand)) {
                continue;
            }

            // Figure out the direction the hand is pointing
            var finger = hand.Fingers[(int)Finger.FingerType.TYPE_INDEX];
            var yaw = finger.Direction.Yaw;
            var pointingDirX = 0;
            var dist = Mathf.Abs(yaw - -Mathf.PI / 2f);
            if (dist < 1f && hand.IsRight) {
                pointingDirX = -1;
            } else if (dist < 4f && hand.IsLeft) {
                pointingDirX = 1;
            }

            // If we're not pointing to the left and right after all, abort mission
            if (pointingDirX == 0) {
                continue;
            }

            // Only proceed if there's been enough movement since the last frame
            var velocity = usePalmVelocity ? hand.PalmVelocity.x : finger.TipVelocity.x;
            if (Mathf.Abs(velocity) < minVelocity) {
                continue;
            }

            // Check if the hand is ping-ponging back and forth, as required for the pointing gesture
            if (lastPointTime != -1f && Time.unscaledTime < lastPointTime + velocitySwitchTime && lastPointingDir == pointingDirX) {
                var lastSign = Mathf.Sign(lastPointVelocityX);
                var sign = Mathf.Sign(velocity);

                // If the velocity x direction has changed, count it as a velocity switch
                if (lastSign != sign) {
                    velocitySwitches++;

                    // If there's been enough velocity switches during the allowed time period, consider this a pointing gesture and fire the event
                    if (velocitySwitches > minVelocitySwitches) {
                        onPointEvent.Invoke(pointingDirX == 1);
                        velocitySwitches = 0;
                        cooldownTimer = cooldownTime;
                    }
                }
            } else {
                velocitySwitches = 0;
            }

            lastPointingDir = pointingDirX;
            lastPointTime = Time.unscaledTime;
            lastPointVelocityX = velocity;
        }
    }

    /// <summary>
    /// Checks whether the given hand is doing a pointing gesture towards the left or right.
    /// </summary>
    /// <param name="hand">the hand to be checked</param>
    /// <returns>true if the hand is pointing to the left or right</returns>
    private bool IsPointingFinger(Hand hand) {
        // Palm must be oriented towards player
        if (hand.PalmNormal.z > -0.5f) {
            return false;
        }

        // Index finger must always be extended for this gesture.
        if (!hand.Fingers[(int)Finger.FingerType.TYPE_INDEX].IsExtended) {
            return false;
        }

        // At least middle and ring finger must not be extended.
        var otherFingers = new Finger.FingerType[] { Finger.FingerType.TYPE_MIDDLE, Finger.FingerType.TYPE_RING };
        var foundNotExtended = false;
        foreach (var fingerType in otherFingers) {
            if (!hand.Fingers[(int)fingerType].IsExtended) {
                foundNotExtended = true;
                break;
            }
        }

        return foundNotExtended;
    }

}