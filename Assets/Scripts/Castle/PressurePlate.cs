using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour {

    public UnityEvent onPressed;
    public UnityEvent onUnpressed;

    private int pressors;

    private void Awake() {
        if (onPressed == null) {
            onPressed = new UnityEvent();
        }
        if (onUnpressed == null) {
            onUnpressed = new UnityEvent();
        }
    }

    private void OnTriggerEnter(Collider collision) {
        if (pressors == 0) {
            onPressed.Invoke();
        }
        pressors++;
    }

    private void OnTriggerExit(Collider collision) {
        pressors--;
        if (pressors <= 0) {
            onUnpressed.Invoke();
            pressors = 0;
        }
    }

}
