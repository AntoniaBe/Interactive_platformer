using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SnappingController : MonoBehaviour {

    public UnityEvent onSnapEvent;
    public GameObject[] grabbables;

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

        if (grabbables.Contains(collider.gameObject)) {
            collider.transform.position = transform.position;
            collider.transform.rotation = transform.rotation;

            collider.gameObject.layer = LayerMask.NameToLayer("SnapIn");
            collider.GetComponent<Grabbable>().IsSnappedIn = true;
            onSnapEvent.Invoke();
            HasSnapped = true;
        }
    }
}
