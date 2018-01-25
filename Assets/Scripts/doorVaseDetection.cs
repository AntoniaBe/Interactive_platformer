using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorVaseDetection : MonoBehaviour {

    public Transform target;
    private Vector3 targetPosition;
    private Vector3 goalPosition = new Vector3(-27.93f, -2.62f, 15.35f);
    public float rotation = 0f;
    private float smooth = 0.2f;
    public bool openDoor = false;
    
	void Start () {
        targetPosition = target.position;
	}
	
	void LateUpdate () {
        //if (targetPosition == goalPosition) {
        if (openDoor) {
            if (transform.rotation.eulerAngles.y > rotation && transform.name.Equals("gate_door_left") || transform.rotation.eulerAngles.y < rotation && transform.name.Equals("gate_door_right"))
            {
                Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -rotation);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smooth);
            }

        }
    }
}
