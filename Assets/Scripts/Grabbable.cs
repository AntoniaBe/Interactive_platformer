using System.Collections;
using UnityEngine;
using Leap;

public class Grabbable : MonoBehaviour {

    private const float EMISSION_INTENSITY = 0.5f;
    private const float EMISSION_FREQUENCY = 3f;

    public HandEvent onGrabEvent;
    public HandEvent onGrabUpdateEvent;
    public HandEvent onUngrabEvent;

    public bool allowMovement = true;
    public bool allowRotation = true;
    public GameObject touchingHand;

    private bool isSnappedIn;
    public bool IsSnappedIn {
        get {
            return isSnappedIn;
        }
        set {
            isSnappedIn = value;
            ApplyEmissionColor(Color.black);
        }
    }

    public bool IsGrabbed { get; private set; }

    private int emissionColorId;
    private float emissionTime;

    private void Awake() {
        if (onGrabEvent == null) {
            onGrabEvent = new HandEvent();
        }
        if (onUngrabEvent == null) {
            onUngrabEvent = new HandEvent();
        }
    }

    private void Start() {
        emissionColorId = Shader.PropertyToID("_EmissionColor");
    }

    private void Update() {
        if (!IsSnappedIn) {
            var intensity = Mathf.Sin(emissionTime * EMISSION_FREQUENCY) * EMISSION_INTENSITY;
            var color = new Color(intensity, intensity, intensity);
            if (touchingHand) {
                color *= Color.yellow;
            }
            ApplyEmissionColor(color);
            emissionTime += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag("hands")) {
            touchingHand = collider.gameObject;
            emissionTime = 1f;
        }
    }


    private void OnTriggerExit(Collider collider) {
        if (collider.gameObject.CompareTag("hands")) {
            touchingHand = null;
        }
    }

    private void ApplyEmissionColor(Color color) {
        var ownRenderer = GetComponent<Renderer>();
        if (ownRenderer) {
            ownRenderer.material.SetColor(emissionColorId, color);
        }

        foreach (var renderer in GetComponentsInChildren<Renderer>()) {
            renderer.material.SetColor(emissionColorId, color);
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
