using UnityEngine;
using System.Collections;

public class DSCartoonEyeLookAt : MonoBehaviour {

	public Transform followThis = null;
	public float speed= 2;
	public float speedRandom= 1;


	private Transform lEye;
	private Transform rEye;
	private Quaternion lookAt;
	private Quaternion eyeCorrection;

	// Use this for initialization
	void Start () {
		rEye = this.transform.Find("rightEye");
		lEye = this.transform.Find("leftEye");
		eyeCorrection.eulerAngles = new Vector3(-90,0,0);
		speed += Random.Range(-speedRandom,speedRandom);

	}
	
	// Update is called once per frame
	void Update () {
		lookAt = Quaternion.LookRotation(followThis.position - rEye.position);
		lookAt *= eyeCorrection; //Eyes Z-Axis Pointing UP instead of ahead CCW rotate around X
		rEye.rotation = Quaternion.Slerp(rEye.rotation,lookAt,speed*Time.deltaTime);

		lookAt = Quaternion.LookRotation(followThis.position - lEye.position);
		lookAt *= eyeCorrection; //Eyes Z-Axis Pointing UP instead of ahead CCW rotate around X
		lEye.rotation = Quaternion.Slerp(lEye.rotation,lookAt,speed*Time.deltaTime);
		//Debug.Log("Rotation = " + lEye.rotation.eulerAngles.ToString());
	}
}
