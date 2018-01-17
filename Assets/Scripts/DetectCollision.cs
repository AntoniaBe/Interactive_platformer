using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour {

    public bool collisionDetected;

    void OnTriggerStay(Collider collider)
    {
                if (collider.gameObject.tag == "hands")
        {
            //If the GameObject has the same tag as specified, output this message in the console

           collisionDetected = true;
           gameObject.GetComponent<Renderer>().material.color = Color.green;
            //Debug.Log("COLLISION");
        }


    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "hands")
        {
            //If the GameObject has the same tag as specified, output this message in the console
           // Debug.Log("Collision Exit");
            collisionDetected = false;
            gameObject.GetComponent<Renderer>().material.color = Color.red;
            //Debug.Log("NO COLLISION");
        }
    }

}
