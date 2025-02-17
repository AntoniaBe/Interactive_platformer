﻿using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Generic trigger area that simply invokes the attached event handlers.
/// </summary>
public class TriggerArea : MonoBehaviour {

    public UnityEvent onEnterArea;
    public UnityEvent onExitArea;

    public string requiredTag;

    private void Awake() {
        if (onEnterArea == null) {
            onEnterArea = new UnityEvent();
        }
        if (onExitArea == null) {
            onExitArea = new UnityEvent();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (string.IsNullOrEmpty(requiredTag) || other.CompareTag(requiredTag)) {
            onEnterArea.Invoke();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (string.IsNullOrEmpty(requiredTag) || other.CompareTag(requiredTag)) {
            onExitArea.Invoke();
        }
    }

}
