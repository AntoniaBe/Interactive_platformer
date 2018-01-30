using UnityEngine;
using UnityEngine.Events;

public class TriggerArea : MonoBehaviour {

    public UnityEvent onEnterArea;
    public UnityEvent onExitArea;

    private void Awake() {
        if (onEnterArea == null) {
            onEnterArea = new UnityEvent();
        }
        if (onExitArea == null) {
            onExitArea = new UnityEvent();
        }
    }

    private void OnTriggerEnter(Collider other) {
        onEnterArea.Invoke();
    }

    private void OnTriggerExit(Collider other) {
        onExitArea.Invoke();
    }

}
