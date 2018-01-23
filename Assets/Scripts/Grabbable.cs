using UnityEngine;

public class Grabbable : MonoBehaviour {

    public HandEvent onGrabEvent;
    public HandEvent onUngrabEvent;

    public GameObject touchingHand;
    public bool isSnappedIn;

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
        GetComponent<Renderer>().material.color = color;
        foreach (var renderer in GetComponentsInChildren<Renderer>()) {
            renderer.material.color = color;
        }
    }

}
