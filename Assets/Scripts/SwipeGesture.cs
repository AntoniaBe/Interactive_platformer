using UnityEngine;
using System.Collections;
using Leap;
using Leap.Unity;
using Leap.Unity.Attributes;
using System.Collections.Generic;


public class SwipeGesture : MonoBehaviour
{
    private LeapServiceProvider leapServiceProvider;
    Hand firstHand;
    Hand secondHand;

    float HandPalmRollMin = 0.5f;
    float HandPalmRollMax = -2.8f;
    float HandPalmYawMin = -0.5f;
    float HandPalmYawMax = -2.6f;
    float PinchMax = 0.3f;
    float maxVelocity = 8f;
    float minVelocity = 8f;
    float yRotationRight = 90;
    float yRotationLeft = -90;

    public new Camera camera;
    float xRotation;

    private void Start()
    {
        leapServiceProvider = FindObjectOfType<LeapServiceProvider>();
        xRotation = camera.transform.eulerAngles.x;
    }


    void Update()
    {

        Frame frame = leapServiceProvider.CurrentFrame;


        if (frame.Hands.Count == 0) {

            return;
        }
        if (frame.Hands.Count > 0)
        {
            List<Hand> hands = frame.Hands;

            firstHand = hands[0];

        }

        if (firstHand.IsRight) {

        if (FingersExtended() && firstHand.PalmVelocity.x > maxVelocity && firstHand.PalmNormal.x < -0.90f && firstHand.PalmNormal.x > -0.99f) {

            camera.transform.eulerAngles = new Vector3(xRotation, yRotationRight, 0);
                Debug.Log("Swipe to Right");     
        }

        if (FingersExtended() && firstHand.PalmVelocity.x < minVelocity && firstHand.PalmNormal.x < -0.96f && firstHand.PalmNormal.x > -0.99f)
        {

           camera.transform.eulerAngles = new Vector3(xRotation, 0, 0);
                Debug.Log("Swipe to Left");
            }
        }

        //Debug.Log(FingersExtended());
    }


    bool FingersExtended()
    {
        if(firstHand.Fingers[0].IsExtended && firstHand.Fingers[1].IsExtended && firstHand.Fingers[2].IsExtended && firstHand.Fingers[3].IsExtended && firstHand.Fingers[4].IsExtended)
        {
            
            return true;
        }
        else return false;
    }

}