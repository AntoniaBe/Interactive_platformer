using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour {

    public bool collisionDetected;

    void OnTriggerEnter(Collider collider) {

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collider.gameObject.tag == "hands") {
            //If the GameObject has the same tag as specified, output this message in the console
            // Debug.Log("Collision!");
            collisionDetected = true;
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
    }


    void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "hands") {
            //If the GameObject has the same tag as specified, output this message in the console
            // Debug.Log("Collision Exit");
            collisionDetected = false;
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
    }

}
