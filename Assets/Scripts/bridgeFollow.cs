using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bridgeFollow : MonoBehaviour {

    public Transform target;

    //rotation -24.611 to 0,
    private static float rStart = -24.611f;
    private static float rEnd = 0f;
    //location -13.61 to -7.7
    private static float pStart = -13.61f;
    private static float pEnd = -7.7f;
    // - 5.91
    private static float pDif = pStart - pEnd;

    float smooth = 2.0f;


    void Start () {
		
	}
	
	void LateUpdate () {
        // if -13.61 then -24.611
        // if -7.7 then 0

        // dif 7.7 and 0 / 7.7
        float targetP = target.transform.position.y;
        float targetDif = targetP - pEnd;
        float dif = targetDif / pDif;
        float rotation = rEnd + dif * rStart;
        Quaternion targetRotation = Quaternion.Euler(0, 0, rotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smooth);
	}
}
