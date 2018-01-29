using UnityEngine;
using Leap;

public class Grabbable : MonoBehaviour {

    public HandEvent onGrabEvent;
    public HandEvent onGrabUpdateEvent;
    public HandEvent onUngrabEvent;

    public bool allowMovement = true;
    public bool allowRotation = true;
    public GameObject touchingHand;
    public bool isSnappedIn;
    public bool IsGrabbed { get; private set; }

    private void Awake() {
        if (onGrabEvent == null) {
            onGrabEvent = new HandEvent();
        }
        if (onUngrabEvent == null) {
            onUngrabEvent = new HandEvent();
        }
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag("hands")) {
            touchingHand = collider.gameObject;
            ApplyMaterialColor(Color.green);
        }
    }


    private void OnTriggerExit(Collider collider) {
        if (collider.gameObject.CompareTag("hands")) {
            touchingHand = null;
            ApplyMaterialColor(Color.white);
        }
    }

    private void ApplyMaterialColor(Color color) {
        var ownRenderer = GetComponent<Renderer>();
        if (ownRenderer) {
            ownRenderer.material.color = color;
        }

        foreach (var renderer in GetComponentsInChildren<Renderer>()) {
            renderer.material.color = color;
        }
    }

    public void StartGrabbing(Hand hand) {
        onGrabEvent.Invoke(hand);
        IsGrabbed = true;
    }

    public void GrabUpdate(Hand hand) {
        onGrabUpdateEvent.Invoke(hand);
    }

    public void StopGrabbing(Hand hand) {
        onUngrabEvent.Invoke(hand);
        IsGrabbed = false;
    }

}
