using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDies : MonoBehaviour {


    private GameObject AI;
    private float minAngle = 0.0F;
    private float maxAngle = -91F;

    void Start () {
        AI = GameObject.Find("NPC");
    }
	

    public void NpcDies()
    {
        AI.GetComponent<Animation>().Stop();
        AI.GetComponent<exAI>().speed = 0;
        float angle = Mathf.LerpAngle(minAngle, maxAngle, Time.time);
        AI.transform.localEulerAngles = new Vector3(-9.75f, 97f, angle);
    }

}
