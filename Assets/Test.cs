using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class Test : MonoBehaviour {

    LeapServiceProvider provider;
    bool closedFist;

    // Use this for initialization
    void Start () {
        provider = FindObjectOfType<LeapServiceProvider>() as LeapServiceProvider;

    }
	
	// Update is called once per frame
	void Update () {


        Frame frame = provider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            if (!hand.Fingers[0].IsExtended && !hand.Fingers[1].IsExtended && !hand.Fingers[2].IsExtended && !hand.Fingers[3].IsExtended && !hand.Fingers[4].IsExtended)
            {
                closedFist = true;
               // Debug.Log("Fist");
            }

            else {
                closedFist = false;
              //  Debug.Log("No Fist");
            }
        }
    }
}
