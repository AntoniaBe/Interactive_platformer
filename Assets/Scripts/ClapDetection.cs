using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Leap;
using Leap.Unity;

public class ClapDetection : MonoBehaviour {

    public UnityEvent onClapEvent;

    public float nearDistance = 3f;
    public float clapDistance = 2f;
    public float probablyClappingCooldown = 0.5f;
    public float clappingCooldown = 1f;
    public float maxAngle = 55f;
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

    private bool AreBothHandsTracked() {
        Frame frame = leapServiceProvider.CurrentFrame;
        return frame.Hands.Count >= 2;
    }

    private bool CanClap() {
        // Cooldown to prevent spam-clapping
        if (Time.unscaledTime - lastClapTime < clappingCooldown) {
            return false;
        }

        return true;
    }

    private bool IsProbablyClapping(float handDistance) {
        Frame frame = leapServiceProvider.CurrentFrame;
        foreach (Hand hand in frame.Hands) {
            // At least the index and middle finger of both hands must be extended
            if (!hand.Fingers[(int) Finger.FingerType.TYPE_INDEX].IsExtended || !hand.Fingers[(int) Finger.FingerType.TYPE_MIDDLE].IsExtended) {
                return false;
            }

            // Hand must be facing the other hand
            // TODO maybe limit this to the x axis only
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

    private void Clap() {
        Debug.Log("*klatsch*");
        onClapEvent.Invoke();
    }

}
