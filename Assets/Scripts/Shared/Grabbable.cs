using UnityEngine;
using Leap;

/// <summary>
/// Component to make an object grabbable by the grab gesture.
/// </summary>
public class Grabbable : MonoBehaviour {

    private const float EMISSION_INTENSITY = 0.2f;
    private const float EMISSION_FREQUENCY = 3f;

    /// <summary>
    /// Event fired when the object is grabbed.
    /// </summary>
    public HandEvent onGrabEvent;

    /// <summary>
    /// Event fired every frame while the object is being grabbed.
    /// </summary>
    public HandEvent onGrabUpdateEvent;

    /// <summary>
    /// Event fired when the object is let go.
    /// </summary>
    public HandEvent onUngrabEvent;

    /// <summary>
    /// Whether this object can be moved by the grab gesture.
    /// </summary>
    public bool allowMovement = true;

    /// <summary>
    /// Whether this object can be rotated by the grab gesture.
    /// </summary>
    public bool allowRotation = true;

    /// <summary>
    /// Whether this object has been snapped into a snapping area.
    /// </summary>
    public bool IsSnappedIn {
        get {
            return isSnappedIn;
        }
        set {
            isSnappedIn = value;
            ApplyEmissionColor(Color.black);
        }
    }

    /// <summary>
    /// The physical hand touching this object (or null if none is touching).
    /// </summary>
    public GameObject TouchingHand { get; set; }

    /// <summary>
    /// Whether this object is being grabbed right now.
    /// </summary>
    public bool IsGrabbed { get; private set; }

    private bool isSnappedIn;
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
        // If the object is not snapped in, show the emission flimmer to make it more obvious
        if (!IsSnappedIn) {
            var intensity = Mathf.Abs(Mathf.Sin(emissionTime * EMISSION_FREQUENCY) * EMISSION_INTENSITY);
            var color = new Color(intensity, intensity, intensity);
            ApplyEmissionColor(color);
            emissionTime += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag("hands")) {
            TouchingHand = collider.gameObject;
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.gameObject.CompareTag("hands")) {
            TouchingHand = null;
        }
    }

    /// <summary>
    /// Applies the emission color to all materials of all renderers in this object.
    /// </summary>
    /// <param name="color">the color to apply</param>
    private void ApplyEmissionColor(Color color) {
        var ownRenderer = GetComponent<Renderer>();
        if (ownRenderer) {
            foreach (var material in ownRenderer.materials) {
                material.SetColor(emissionColorId, color);
            }
        }

        foreach (var renderer in GetComponentsInChildren<Renderer>()) {
            foreach (var material in renderer.materials) {
                material.SetColor(emissionColorId, color);
            }
        }
    }

    /// <summary>
    /// Fires the grabbing start event.
    /// </summary>
    /// <param name="hand">the hand grabbing the object/param>
    public void StartGrabbing(Hand hand) {
        onGrabEvent.Invoke(hand);
        IsGrabbed = true;
    }

    /// <summary>
    /// Fires the grabbing update event.
    /// </summary>
    /// <param name="hand">the hand grabbing the object/param>
    public void GrabUpdate(Hand hand) {
        onGrabUpdateEvent.Invoke(hand);
    }

    /// <summary>
    /// Fires the grabbing stop event.
    /// </summary>
    /// <param name="hand">the hand grabbing the object/param>
    public void StopGrabbing(Hand hand) {
        onUngrabEvent.Invoke(hand);
        IsGrabbed = false;
    }

}
