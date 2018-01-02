using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappingController : MonoBehaviour {

    float transparency = 0.1f;
    DetectCollision detectCollisionScript;
    public Vector3 posOfTrigger;

    private void Start()
    {
        posOfTrigger = transform.position;
        
    }

    void OnTriggerStay(Collider col)
    {
        //Change Col.gameObject.tag String to wanted collision object tag
        if (col.gameObject.name == "grabObject")
        {
            //Change posi vector to target position of snapping
            //Vector3 posi = new Vector3(0f, 0f, 00f);
            col.gameObject.layer = LayerMask.NameToLayer("SnapIn");
            col.isTrigger = false;
            col.transform.position = posOfTrigger;
            StartCoroutine(WaitAndDestroy());
        }
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }


}
