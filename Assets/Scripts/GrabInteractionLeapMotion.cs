using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class GrabInteractionLeapMotion : MonoBehaviour {

    LeapServiceProvider provider;

    public bool closedFist;
    public bool getDetectCollision;

    public GameObject[] detectCollisions;

    public GameObject rightHand;

    void Start () {
        provider = FindObjectOfType<LeapServiceProvider>() as LeapServiceProvider;

        //Save all objects with the tag "grab" in an array
        detectCollisions = GameObject.FindGameObjectsWithTag("grab");
    }
	

	void Update () {

        Frame frame = provider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            //If thumb and index finger are not extended, it should be recognized as a fist (grab gesture)
            //Not using all fingers because thumb and index finger are better recognized by the leap motion
            // && !hand.Fingers[2].IsExtended && !hand.Fingers[3].IsExtended && !hand.Fingers[4].IsExtended
            if (!hand.Fingers[0].IsExtended && !hand.Fingers[1].IsExtended )
            {
                closedFist = true;
            }

            else {
                closedFist = false;
            }
        }


        for (int i = 0; i < detectCollisions.Length; i++)
        {
            //get information if a collision between hand and an grabbable object took place
            getDetectCollision = detectCollisions[i].GetComponent<DetectCollision>().collisionDetected;

            if (getDetectCollision)
            {
                getDetectCollision = true;

                if (closedFist)
                {
                    getDetectCollision = true;
                    closedFist = true;

                    //when collision and closed fist, grabbable object should take position of hand (palm)
                    detectCollisions[i].transform.position = rightHand.transform.position;
                }

            }
 
        }
    }
}
