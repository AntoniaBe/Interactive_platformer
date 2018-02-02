using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Pressure Plate that keeps track off how many things are touching it and calls assigned event handlers on press and release.
/// </summary>
public class PressurePlate : MonoBehaviour {

    /// <summary>
    /// Called when the pressure plate has been pressed.
    /// </summary>
    public UnityEvent onPressed;

    /// <summary>
    /// Called when the pressure plate has been released.
    /// </summary>
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
        // Only invoke the press event on the first press
        if (pressors == 0) {
            onPressed.Invoke();
        }
        pressors++;
    }

    private void OnTriggerExit(Collider collision) {
        pressors--;
        // Only invoke the release event when all touching objects have left
        if (pressors <= 0) {
            onUnpressed.Invoke();
            pressors = 0;
        }
    }

}
