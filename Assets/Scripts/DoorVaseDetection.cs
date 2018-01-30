using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorVaseDetection : MonoBehaviour {

    public Transform target;
    public Transform goal;
    private Vector3 goalPosition;
    private Vector3 targetPosition;
    public float smooth = 0.5f;
    public Vector3 targetRotation;
    public GameObject[] spikes;

    private void Start() {
        goalPosition = goal.transform.position;
    }

    /// <summary>
    /// Get newest position of target object to check if the distance between goal and target is smaller than 0.1f.
    /// If so rotate both, gate_door_left and gate_door_right, to their goal rotation. And deactivate "door_spikes"
    /// </summary>
    void LateUpdate() {
        targetPosition = target.position;
        if (Vector3.Distance(targetPosition, goalPosition) < 0.1f) {
            if (spikes != null) {
                foreach (var spike in spikes) {
                    spike.SetActive(false);
                }
                spikes = null;
            }
            Quaternion targetQuaternion = Quaternion.Euler(targetRotation);
            if (Quaternion.Angle(transform.rotation, targetQuaternion) > 1f) {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetQuaternion, Time.deltaTime * smooth);
            }

        }
    }
}
