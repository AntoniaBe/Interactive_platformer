using UnityEngine;
using Leap;
using Leap.Unity;

public class PointingGesture : MonoBehaviour {

    public BooleanEvent onPointEvent;

    public float minVelocity = 1f;
    public float velocitySwitchTime = 1f;
    public int minVelocitySwitches = 2;
    public float cooldownTime = 1f;
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
        if (cooldownTimer > 0f) {
            cooldownTimer -= Time.unscaledDeltaTime;
            return;
        }

        Frame frame = leapServiceProvider.CurrentFrame;
        foreach (var hand in frame.Hands) {
            if (!IsPointingFinger(hand)) {
                continue;
            }

            var finger = hand.Fingers[(int) Finger.FingerType.TYPE_INDEX];
            var yaw = finger.Direction.Yaw;
            var pointingDirX = 0;
            var dist = Mathf.Abs(yaw - -Mathf.PI / 2f);
            if (dist < 1f && hand.IsRight) {
                pointingDirX = -1;
            } else if (dist < 4f && hand.IsLeft) {
                pointingDirX = 1;
            }

            if (pointingDirX == 0) {
                continue;
            }

            var velocity = usePalmVelocity ? hand.PalmVelocity.x : finger.TipVelocity.x;

            if (Mathf.Abs(velocity) < minVelocity) {
                continue;
            }

            if (lastPointTime != -1f && Time.unscaledTime < lastPointTime + velocitySwitchTime && lastPointingDir == pointingDirX) {
                var lastSign = Mathf.Sign(lastPointVelocityX);
                var sign = Mathf.Sign(velocity);
                if (lastSign != sign) {
                    velocitySwitches++;
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

    private bool IsPointingFinger(Hand hand) {
        // Palm must be oriented towards player TODO testme
        if (hand.PalmNormal.z > -0.5f) {
            return false;
        }

        // Index finger must always be extended for this gesture.
        if (!hand.Fingers[(int) Finger.FingerType.TYPE_INDEX].IsExtended) {
            return false;
        }

        // At least middle and ring finger must not be extended.
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

}