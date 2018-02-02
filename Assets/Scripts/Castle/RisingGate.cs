using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingGate : MonoBehaviour {

    public float movementScale = 3.5f;

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
