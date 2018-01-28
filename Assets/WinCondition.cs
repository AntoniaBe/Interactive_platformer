using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{

    private GameObject AI;
    // Use this for initialization
    void Start()
    {
        AI = GameObject.Find("NPC");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            AI.GetComponent<Animation>().Stop();
            AI.GetComponent<exAI>().speed = 0;
        }

    }

}
