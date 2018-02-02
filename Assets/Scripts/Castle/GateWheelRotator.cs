using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class GateWheelRotator : MonoBehaviour {

    public float openingSpeed = 0.01f;
    public float revertSpeed = 0.05f;
    public float revertTime = 0.5f;
    public float minRotation = 1f;
    public RisingGate[] gates;
    public bool isLocked;

    private float previousAngle;
    private float revertTimer;

    public void GrabUpdate(Hand hand) {
        var oldEuler = transform.rotation.eulerAngles;
        var handPos = hand.PalmPosition.ToVector3();
        float targetAngle = Mathf.Atan2(handPos.y - transform.position.y, transform.position.x - handPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(targetAngle, 90f, -90f);

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
        if (revertTimer <= 0f && !isLocked) {
            foreach (var gate in gates) {
                gate.normalizedValue -= revertSpeed;
            }
        } else {
            revertTimer -= Time.deltaTime;
        }
    }

}
