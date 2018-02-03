using UnityEngine;
using Leap;
using Leap.Unity;

/// <summary>
/// Allows a wheel to be rotated by grabbing it with the hand and moving the hand around it.
/// </summary>
public class GateWheelRotator : MonoBehaviour {

    /// <summary>
    /// The speed at which the connected gate will rise when this wheel is rotated.
    /// </summary>
    public float openingSpeed = 0.01f;

    /// <summary>
    /// The speed at which the connected gate will fall if this wheel is no longer rotated.
    /// </summary>
    public float revertSpeed = 0.05f;

    /// <summary>
    /// The delay before the connected gate will fall if this wheel is no longer rotated.
    /// </summary>
    public float revertTime = 0.5f;

    /// <summary>
    /// The minimum amount of rotation on this wheel for it to affect the gate.
    /// </summary>
    public float minRotation = 1f;

    /// <summary>
    /// The gates to be affected by this wheel.
    /// </summary>
    public RisingGate[] gates;

    /// <summary>
    /// Whether the gate is locked in position.
    /// </summary>
    public bool isLocked;

    private float previousAngle;
    private float revertTimer;

    /// <summary>
    /// Called on every frame while the object is being grabbed.
    /// </summary>
    /// <param name="hand">the hand the object is being grabbed by</param>
    public void GrabUpdate(Hand hand) {
        var oldEuler = transform.rotation.eulerAngles;
        var handPos = hand.PalmPosition.ToVector3();
        float targetAngle = Mathf.Atan2(handPos.y - transform.position.y, transform.position.x - handPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(targetAngle, 90f, -90f);

        // Rise the gate if the wheel has been rotated enough
        float rotationDelta = Mathf.Abs(targetAngle - previousAngle);
        if (rotationDelta > minRotation) {
            foreach (var gate in gates) {
                gate.normalizedValue += openingSpeed;
            }
            revertTimer = revertTime;
        }

        previousAngle = targetAngle;
    }

    private void Update() {
        // Cause the gate to fall after a short delay with no rotation
        if (revertTimer <= 0f && !isLocked) {
            foreach (var gate in gates) {
                gate.normalizedValue -= revertSpeed;
            }
        } else {
            revertTimer -= Time.deltaTime;
        }
    }

}
