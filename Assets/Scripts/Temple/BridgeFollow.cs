﻿using UnityEngine;

/// <summary>
/// Causes a bridge to rotate based on a target object's position.
/// </summary>
public class BridgeFollow : MonoBehaviour {

    /// <summary>
    /// The target object to follow.
    /// </summary>
    public Transform target;

    private static float rotStart = -24.611f;
    private static float rotEnd = 0f;
    private static float posStart = -13.61f;
    private static float posEnd = -7.7f;
    private static float posDif = posStart - posEnd;
    private float smooth = 2.0f;

    /// <summary>
    /// Use the target object to calculate the rotation of the bridge object, with its Y position, 
    /// in order to make the bridge object follow the target object once moved.
    /// </summary>
    private void LateUpdate() {
        float targetP = target.transform.position.y;
        float targetDif = targetP - posEnd;
        float dif = targetDif / posDif;
        float rotation = rotEnd + dif * rotStart;
        Quaternion targetRotation = Quaternion.Euler(0, 0, rotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smooth);
    }
}
