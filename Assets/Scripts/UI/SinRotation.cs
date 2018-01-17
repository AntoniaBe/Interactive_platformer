using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinRotation : MonoBehaviour {

    public float frequency = 1f;
    public float amplitude = 0.1f;
    public bool useUnscaledTime;
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
