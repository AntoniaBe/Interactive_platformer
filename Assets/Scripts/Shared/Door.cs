using UnityEngine;

/// <summary>
/// Provides functionality for opening or closing a door.
/// </summary>
public class Door : MonoBehaviour {

    /// <summary>
    /// The rotation the door should be at when it's open.
    /// </summary>
    public Vector3 openRotation;

    /// <summary>
    /// The rotation the door should be at when it's closed.
    /// </summary>
    public Vector3 closeRotation;

    /// <summary>
    /// The speed at which the door opens.
    /// </summary>
    public float openSpeed = 0.2f;

    /// <summary>
    /// The speed at which the door closes.
    /// </summary>
    public float closeSpeed = 0.3f;

    /// <summary>
    /// The state of the door. Setting this will automatically cause the opening or close animation to run.
    /// </summary>
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
