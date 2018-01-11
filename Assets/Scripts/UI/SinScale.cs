using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinScale : MonoBehaviour {

    public float frequency = 1f;
    public float amplitude = 0.1f;
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
