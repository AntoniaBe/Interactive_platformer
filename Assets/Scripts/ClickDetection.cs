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
    public float minVelocity = 0.5f;

    private LeapServiceProvider leapServiceProvider;
    private Camera mainCamera;
    private float lastClickTime;

    private void Start() {
        leapServiceProvider = FindObjectOfType<LeapServiceProvider>();
        mainCamera = Camera.main;
    }

    private void Update() {
        // Don't bother doing checks if we're not allowed to click
        if (!CanClick()) {
            return;
        }

        var frame = leapServiceProvider.CurrentFrame;
        foreach (var hand in frame.Hands) {
            if (IsClickGesture(hand)) {
                var indexFinger = hand.Finger((int) Finger.FingerType.TYPE_INDEX);
                var screenPoint = mainCamera.WorldToScreenPoint(indexFinger.TipPosition.ToVector3());
                SimulateButtonClick(screenPoint);
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
        if (!IsPointingGesture(hand)) {
            return false;
        }

        var indexFinger = hand.Finger((int) Finger.FingerType.TYPE_INDEX);
        return indexFinger.TipVelocity.z > minVelocity;
    }

    private bool IsPointingGesture(Hand hand) {
        foreach (var finger in hand.Fingers) {
            if (finger.IsExtended != (finger.Type == Finger.FingerType.TYPE_INDEX)) {
                return false;
            }
        }

        return true;
    }

    private void SimulateButtonClick(Vector3 position) {
        var pointer = new PointerEventData(EventSystem.current) {
            position = position,
            pressPosition = position
        };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, results);
        if (results.Count > 0) {
            var button = results.Select(t => t.gameObject.GetComponent<Button>()).FirstOrDefault(t => t);
            if (button) {
                ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerClickHandler);
            }
        }
    }

}
