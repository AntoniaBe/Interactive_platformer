using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour {

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "scenery_lava")
        {
            Debug.Log("collision! GAME OVER");
            //Add Game Over Screen initiation
        }
        if (col.gameObject.name == "scenery_flagPost")
        {
            Debug.Log("Level Cleared! Victory");
            //Add Vicory Screen initiation
        }
    }
}
