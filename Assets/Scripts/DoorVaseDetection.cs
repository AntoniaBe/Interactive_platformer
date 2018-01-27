using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorVaseDetection : MonoBehaviour {

    public Transform target;
    public Transform goal;
    private Vector3 targetPosition;
    private Vector3 goalPosition;
    public float rotation = 0f;
    private float smooth = 0.2f;
    private GameObject trigger;

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
            if (transform.rotation.eulerAngles.y > rotation && transform.name.Equals("gate_door_left") || transform.rotation.eulerAngles.y < rotation && transform.name.Equals("gate_door_right"))
            {
                Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -rotation);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smooth);
                trigger.SetActive(false);
            }

        }
    }
}
