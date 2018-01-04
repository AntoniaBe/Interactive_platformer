using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Leap;
using Leap.Unity;

public class ClickDetection : MonoBehaviour {

    public float clickCooldown = 1f;
    public float minVelocity = 0.1f;

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
            var pointingFingerType = GetPointingFinger(hand);
            if (!pointingFingerType.HasValue) {
                return;
            }

            var pointingFinger = hand.Fingers[(int) pointingFingerType];
            var screenPoint = mainCamera.WorldToScreenPoint(pointingFinger.TipPosition.ToVector3());
            var pointer = new PointerEventData(EventSystem.current) {
                position = screenPoint,
                pressPosition = screenPoint
            };

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

    private bool CanClick() {
        // Cooldown to prevent spam-clicking
        if (Time.unscaledTime - lastClickTime < clickCooldown) {
            return false;
        }

        return true;
    }

    private bool IsClickGesture(Hand hand) {
        return hand.Fingers[(int) Finger.FingerType.TYPE_INDEX].TipVelocity.z > minVelocity || hand.Fingers[(int) Finger.FingerType.TYPE_MIDDLE].TipVelocity.z > minVelocity;
    }

    public Finger.FingerType? GetPointingFinger(Hand hand) {
        if (hand.Fingers[(int) Finger.FingerType.TYPE_MIDDLE].IsExtended) {
            return Finger.FingerType.TYPE_MIDDLE;
        }

        if (hand.Fingers[(int) Finger.FingerType.TYPE_INDEX].IsExtended) {
            return Finger.FingerType.TYPE_INDEX;
        }

        return null;
    }

    private Button GetPointerButton(PointerEventData pointer) {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, results);
        if (results.Count > 0) {
            return results.Select(t => t.gameObject.GetComponent<Button>()).FirstOrDefault(t => t);
        }

        return null;
    }

    private void SimulateButtonClick(PointerEventData pointer) {
        var button = GetPointerButton(pointer);
        if (button) {
            ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerClickHandler);
        }
    }

}
