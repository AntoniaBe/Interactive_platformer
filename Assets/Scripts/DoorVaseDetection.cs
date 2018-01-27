using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorVaseDetection : MonoBehaviour {

    public Transform target;
    public Transform goal;
    private Vector3 targetPosition;
    private Vector3 goalPosition;
    private float smooth = 0.2f;
    private GameObject trigger;
    public Vector3 targetRotation;

    /// <summary>
    /// Initialise goalPosition to the public goal object and find the "door_spikes" object for later use
    /// </summary>
    void Start () {
        goalPosition = goal.position;
        trigger = GameObject.Find("door_spikes");
	}
	
    /// <summary>
    /// Get newest position of target object to check if the distance between goal and target is smaller than 0.1f.
    /// If so rotate both, gate_door_left and gate_door_right, to their goal rotation. And deactivate "door_spikes"
    /// </summary>
	void LateUpdate () {
        targetPosition = target.position;
        if (Vector3.Distance(targetPosition, goalPosition) < 0.1f)
        {
            Quaternion targetQuaternion = Quaternion.Euler(targetRotation);
            if (Quaternion.Angle(transform.rotation, targetQuaternion) > 1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetQuaternion, Time.deltaTime * smooth);
                trigger.SetActive(false);
            }

        }
    }
}
