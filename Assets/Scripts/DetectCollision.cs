using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour {

    public bool collisionDetected;
    public bool snapIn;




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

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "snapIn")
        {
            snapIn = true;
        }

      // Debug.Log(gameObject.GetComponent<Collider>().bounds.center);
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
