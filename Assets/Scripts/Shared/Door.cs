using UnityEngine;

public class Door : MonoBehaviour {

    public Vector3 openRotation;
    public Vector3 closeRotation;
    public float openSpeed = 0.2f;
    public float closeSpeed = 0.3f;

    public bool IsOpen { get; set; }

    private Quaternion openQuaternion;
    private Quaternion closeQuaternion;

    private void Start() {
        openQuaternion = Quaternion.Euler(openRotation);
        closeQuaternion = Quaternion.Euler(closeRotation);
    }

    private void Update() {
        if (IsOpen) {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, openQuaternion, Time.deltaTime * openSpeed);
        } else {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, closeQuaternion, Time.deltaTime * closeSpeed);
        }
    }

}
