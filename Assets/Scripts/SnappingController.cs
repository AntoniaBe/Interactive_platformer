using System.Collections;
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

    private void OnTriggerEnter(Collider col) {
        if (HasSnapped) {
            return;
        }

        if (grabbables.Contains(col.gameObject)) {
            col.transform.position = transform.position;
            col.transform.rotation = transform.rotation;

            col.gameObject.layer = LayerMask.NameToLayer("SnapIn");
            col.GetComponent<Grabbable>().IsSnappedIn = true;
            onSnapEvent.Invoke();
            HasSnapped = true;
        }
    }
}
