using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBridge : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {

        Debug.Log("Test");
        if (col.gameObject.tag == "IgnoreCollision")
        {
            Debug.Log("Test");
          Vector3 posi = new Vector3(-1.09f, 2.35f, -1.82f);
          col.transform.position= posi;
           Destroy(col.gameObject.GetComponent<moveObject>());
        }
    }
}
