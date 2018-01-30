using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour {

    public UnityEvent onPressed;
    public UnityEvent onUnpressed;

    private void Awake() {
        if (onPressed == null) {
            onPressed = new UnityEvent();
        }
        if (onUnpressed == null) {
            onUnpressed = new UnityEvent();
        }
    }

    private void OnTriggerEnter(Collider collision) {
        onPressed.Invoke();
    }

    private void OnTriggerExit(Collider collision) {
        onUnpressed.Invoke();
    }

}
