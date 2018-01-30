using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SnappingController : MonoBehaviour {

    public UnityEvent onSnapEvent;
    public GameObject[] grabbables;

    private void Awake() {
        if (onSnapEvent == null) {
            onSnapEvent = new UnityEvent();
        }
    }

    private IEnumerator OnTriggerEnter(Collider col) {
        if (grabbables.Contains(col.gameObject)) {
            col.transform.position = transform.position;
            col.transform.rotation = transform.rotation;

            col.gameObject.layer = LayerMask.NameToLayer("SnapIn");
            col.GetComponent<Grabbable>().isSnappedIn = true;
            onSnapEvent.Invoke();
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }
}
