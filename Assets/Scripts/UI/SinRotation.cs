using UnityEngine;

/// <summary>
/// Rotates the attached object based on a sinus curve.
/// </summary>
public class SinRotation : MonoBehaviour {

    /// <summary>
    /// The frequency to run the animation at (how fast it ping-pongs).
    /// </summary>
    public float frequency = 1f;

    /// <summary>
    /// The amplitude to run the animation at (how much it rotates back and forth).
    /// </summary>
    public float amplitude = 0.1f;

    /// <summary>
    /// Whether unscaled time should be used for this animation.
    /// </summary>
    public bool useUnscaledTime;

    /// <summary>
    /// Vector mask to specify which components are affected by the rotation. The resulting rotation is multiplied by this value.
    /// </summary>
    public Vector3 angleMask = Vector3.forward;

    private Vector3 eulerAngles;

    private void Start() {
        eulerAngles = transform.rotation.eulerAngles;
    }

    private void Update() {
        float time = useUnscaledTime ? Time.unscaledTime : Time.time;
        float v = Mathf.Sin(time * frequency) * amplitude;

        transform.rotation = Quaternion.Euler(eulerAngles + angleMask * v);
    }

}
