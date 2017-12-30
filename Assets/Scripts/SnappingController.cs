using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappingController : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        //Change Col.gameObject.tag String to wanted collision object tag
        if (col.gameObject.tag == "Event Collision Tag")
        {
            //Change posi vector to target position of snapping
            Vector3 posi = new Vector3(0f, 0f, 00f);
            col.transform.position = posi;

            //Delete if working with Leap Motion, moveObject script just for testing without Leap Motion
            Destroy(col.gameObject.GetComponent<moveObject>());
        }
    }

}
