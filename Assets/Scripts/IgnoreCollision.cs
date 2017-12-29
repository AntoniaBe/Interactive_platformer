using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour {


    public GameObject hands;
    public GameObject other;

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "hands")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            Debug.Log("NPC collision");
        }

    }


}
