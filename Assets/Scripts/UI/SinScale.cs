using UnityEngine;

/// <summary>
/// Scales the attached object based on a sinus curve.
/// </summary>
public class SinScale : MonoBehaviour {

    /// <summary>
    /// The frequency to run the animation at (how fast it ping-pongs).
    /// </summary>
    public float frequency = 1f;

    /// <summary>
    /// The amplitude to run the animation at (how much it grows and shrinks).
    /// </summary>
    public float amplitude = 0.1f;

    /// <summary>
    /// Whether unscaled time should be used for this animation.
    /// </summary>
    public bool useUnscaledTime;

    private Vector3 scale;

    private void Start() {
        scale = transform.localScale;
    }

    private void Update() {
        float time = useUnscaledTime ? Time.unscaledTime : Time.time;
        float v = Mathf.Sin(time * frequency) * amplitude;

        transform.localScale = scale + new Vector3(v, v, v);
    }

}
