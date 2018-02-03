using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Leap;
using Leap.Unity;

/// <summary>
/// Leap Gesture detecting a click on a UI button.
/// </summary>
public class ClickGesture : MonoBehaviour {

    /// <summary>
    /// The cooldown delay after each click.
    /// </summary>
    public float clickCooldown = 1f;

    /// <summary>
    /// The minimum velocity for a movement to be considered a click.
    /// </summary>
    public float minVelocity = 5f;

    private LeapServiceProvider leapServiceProvider;
    private Camera mainCamera;
    private float lastClickTime;
    private Button lastHoverButton;

    private void Start() {
        leapServiceProvider = FindObjectOfType<LeapServiceProvider>();
        mainCamera = Camera.main;
    }

    private void Update() {
        var frame = leapServiceProvider.CurrentFrame;
        foreach (var hand in frame.Hands) {
            // Find the best suited finger for the click
            var pointingFingerType = GetPointingFinger(hand);
            if (!pointingFingerType.HasValue) {
                continue;
            }

            // Only allow clicks if the hand is oriented towards the screen
            if (hand.PalmNormal.z < 0.4f || hand.PalmNormal.y >= 0f || Mathf.Abs(hand.PalmNormal.x) >= 0.75f) {
                continue;
            }

            // Map the finger position to a point on screen
            var pointingFinger = hand.Fingers[(int)pointingFingerType];
            var screenPoint = mainCamera.WorldToScreenPoint(pointingFinger.TipPosition.ToVector3());
            var pointer = new PointerEventData(EventSystem.current) {
                position = screenPoint,
                pressPosition = screenPoint
            };

            // Find a button on screen and simulate a click or hover event.
            var button = GetPointerButton(pointer);
            if (CanClick() && IsClickGesture(hand)) {
                SimulateButtonClick(pointer);
            } else if (button != lastHoverButton) {
                if (lastHoverButton) {
                    ExecuteEvents.Execute(lastHoverButton.gameObject, pointer, ExecuteEvents.pointerExitHandler);
                }
                lastHoverButton = button;
                if (lastHoverButton) {
                    ExecuteEvents.Execute(lastHoverButton.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
                }
            }
        }
    }

    /// <summary>
    /// Checks if the gesture is allowed to perform a click.
    /// </summary>
    /// <returns>true if a click is allowed</returns>
    private bool CanClick() {
        // Cooldown to prevent spam-clicking
        if (Time.unscaledTime - lastClickTime < clickCooldown) {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if the hand is performing a click right now.
    /// </summary>
    /// <param name="hand">the hand to be checked</param>
    /// <returns></returns>
    private bool IsClickGesture(Hand hand) {
        // Either index or middle finger velocity must meet the minimum
        return hand.Fingers[(int)Finger.FingerType.TYPE_INDEX].TipVelocity.z > minVelocity || hand.Fingers[(int)Finger.FingerType.TYPE_MIDDLE].TipVelocity.z > minVelocity;
    }

    /// <summary>
    /// Returns the best finger to use for finding the click position.
    /// </summary>
    /// <param name="hand">the hand to be checked</param>
    /// <returns>the finger best suited for the click, or null if no finger can click</returns>
    public Finger.FingerType? GetPointingFinger(Hand hand) {
        if (hand.Fingers[(int)Finger.FingerType.TYPE_MIDDLE].IsExtended) {
            return Finger.FingerType.TYPE_MIDDLE;
        }

        if (hand.Fingers[(int)Finger.FingerType.TYPE_INDEX].IsExtended) {
            return Finger.FingerType.TYPE_INDEX;
        }

        return null;
    }

    /// <summary>
    /// Performs an EventSystem Raycast to find a button below the simulated pointer.
    /// </summary>
    /// <param name="pointer">the simualted pointer data</param>
    /// <returns>the button below the pointer or null if there is none</returns>
    private Button GetPointerButton(PointerEventData pointer) {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, results);
        if (results.Count > 0) {
            return results.Select(t => t.gameObject.GetComponentInParent<Button>()).FirstOrDefault(t => t);
        }

        return null;
    }

    /// <summary>
    /// Simulates a button click and activates the cooldown period.
    /// </summary>
    /// <param name="pointer">the simualted pointer data</param>
    private void SimulateButtonClick(PointerEventData pointer) {
        var button = GetPointerButton(pointer);
        if (button) {
            ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerClickHandler);
            lastClickTime = Time.unscaledTime;
        }
    }

    /// <summary>
    /// Activates the cooldown period manually. Used when opening menus to prevent accidental clicks.
    /// </summary>
    /// <param name="time">the amount of time to wait before accepting clicks again</param>
    public void WaitBeforeDetection(float time) {
        lastClickTime = Time.unscaledTime - clickCooldown + time;
    }

}
