using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Trigger to snap in configured grabbables once they enter the area.
/// </summary>
public class SnappingController : MonoBehaviour {

    /// <summary>
    /// Event fired once a grabbable has been snapped into this area.
    /// </summary>
    public UnityEvent onSnapEvent;

    /// <summary>
    /// Grabbable objects allowed to snap into this area.
    /// </summary>
    public GameObject[] grabbables;

    /// <summary>
    /// Whether an object has been snapped into this area.
    /// </summary>
    public bool HasSnapped { get; private set; }

    private void Awake() {
        if (onSnapEvent == null) {
            onSnapEvent = new UnityEvent();
        }
    }

    private void OnTriggerEnter(Collider collider) {
        if (HasSnapped) {
            return;
        }

        // Only snap the object in if it's allowed in this area
        if (grabbables.Contains(collider.gameObject)) {
            // Move the object to the correct position and rotation
            collider.transform.position = transform.position;
            collider.transform.rotation = transform.rotation;

            // Change the layer of the object to enable collisions and mark it as snapped in to prevent further grabbing
            collider.gameObject.layer = LayerMask.NameToLayer("SnapIn");
            collider.GetComponent<Grabbable>().IsSnappedIn = true;

            // Fire the event and update state
            onSnapEvent.Invoke();
            HasSnapped = true;
        }
    }
}
