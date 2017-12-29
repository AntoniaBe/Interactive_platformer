using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class Test : MonoBehaviour {

    LeapServiceProvider provider;
    public bool closedFist;

    DetectCollision detectCollisionScript;

    public bool getDetectCollision;

    public GameObject[] detectCollisions;

    public GameObject rightHand;

    // Use this for initialization
    void Start () {
        provider = FindObjectOfType<LeapServiceProvider>() as LeapServiceProvider;

        detectCollisions = GameObject.FindGameObjectsWithTag("grab");



    }
	
	// Update is called once per frame
	void Update () {


        //Debug.Log(rightHand.transform.position);

        Frame frame = provider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            // && !hand.Fingers[2].IsExtended && !hand.Fingers[3].IsExtended && !hand.Fingers[4].IsExtended
            if (!hand.Fingers[0].IsExtended && !hand.Fingers[1].IsExtended )
            {
                closedFist = true;
               // Debug.Log("Fist");
            }

            else {
                closedFist = false;
                //  Debug.Log("No Fist");
            }
        }


        for (int i = 0; i < detectCollisions.Length; i++)
        {
            getDetectCollision = detectCollisions[i].GetComponent<DetectCollision>().collisionDetected;
            //Debug.Log(detectCollisions[i].transform.position);

            if (getDetectCollision)
            {
                getDetectCollision = true;

                if (closedFist)
                {
                    getDetectCollision = true;
                    closedFist = true;

                    //detectCollisions[i].transform.position = rightHand.transform.position;
                    detectCollisions[i].transform.position = rightHand.transform.position;

                   if (detectCollisions[i].transform.childCount > 0) {

                        Debug.Log("child!");
                    }


                   // Debug.Log("detectCollisions[i].transform.position " + detectCollisions[i].transform.position );
                    //Debug.Log("rightHand.transform.position " + rightHand.transform.position);

                }

                // Debug.Log("Closed Fist and Collision");
            }
 
        }






    }
}
