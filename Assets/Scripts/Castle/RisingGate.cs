using UnityEngine;

/// <summary>
/// Controls the movement of a gate that can be raised or lowered.
/// </summary>
public class RisingGate : MonoBehaviour {

    /// <summary>
    /// The amount of units the gate will be raised.
    /// </summary>
    public float movementScale = 3.5f;

    /// <summary>
    /// Value between 0 and 1 describing how far the gate has been raised.
    /// </summary>
    [Range(0f, 1f)]
    public float normalizedValue;

    private Vector3 originalPos;

    private void Start() {
        originalPos = transform.position;
    }

    private void Update() {
        normalizedValue = Mathf.Clamp01(normalizedValue);
        transform.position = originalPos + new Vector3(0f, normalizedValue * movementScale, 0f);
    }

}
