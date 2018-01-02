using UnityEngine;
using System.Collections;
using Leap;
using Leap.Unity;
using Leap.Unity.Attributes;
using System.Collections.Generic;


public class SwipeGesture : MonoBehaviour
{

    public float HandPalmYaw;
    public float HandPalmRoll;
    public float pinch;
    private LeapServiceProvider leapServiceProvider;
    Hand firstHand;
    Hand secondHand;

    public float HandPalmRollMin = 0.5f;
    public float HandPalmRollMax = -2.8f;
    public float HandPalmYawMin = -0.5f;
    public float HandPalmYawMax = -2.6f;
    public float PinchMax = 0.3f;
    public float minVelocity = 16f;
    public float maxVelocity = -16f;
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

        HandPalmRoll = firstHand.PalmNormal.Roll;
        HandPalmYaw = firstHand.PalmNormal.Yaw;


        //Debug.Log(firstHand.PalmNormal.x);

        if (firstHand.PalmNormal.x < -0.96f && firstHand.PalmNormal.x > -0.99f)
        {
            //Debug.Log("SUP");

        }


        pinch = firstHand.PinchStrength; //turn radio on

        // Debug.Log(firstHand.PalmVelocity.x);

        if (firstHand.IsRight) {

        if (firstHand.Fingers[0].IsExtended && firstHand.Fingers[1].IsExtended && firstHand.Fingers[2].IsExtended && firstHand.Fingers[3].IsExtended && firstHand.Fingers[4].IsExtended && firstHand.PalmVelocity.x > minVelocity && firstHand.PalmNormal.x < -0.90f && firstHand.PalmNormal.x > -0.99f) {

            camera.transform.eulerAngles = new Vector3(xRotation, yRotationRight, 0);
         
        }

        if (firstHand.Fingers[0].IsExtended && firstHand.Fingers[1].IsExtended && firstHand.Fingers[2].IsExtended && firstHand.Fingers[3].IsExtended && firstHand.Fingers[4].IsExtended && firstHand.PalmVelocity.x < maxVelocity && firstHand.PalmNormal.x < -0.96f && firstHand.PalmNormal.x > -0.99f)
        {

           camera.transform.eulerAngles = new Vector3(xRotation, 0, 0);
        }
        }


        //Debug.Log("RightHand: " + RightHand);
        //Debug.Log("LeftHand: " + LeftHand);
        //Debug.Log("Pitch: " + HandPalmPitch);
        //Debug.Log("Roll: " + HandPalmRoll);
        //Debug.Log("Yaw: " + HandPalmYaw);
        //Debug.Log("Wrist: " + HandWristRot);
        //Debug.Log("Pinch: " + pinch);
        //Debug.Log("PalmdirectionRoll: " + HandPalmDirectionRoll);
        //Debug.Log("RightHandPos: " + VectorHandPosRight);
        //Debug.Log("RightHand: " + VectorHandDirRight);
        //Debug.Log("DistanceBetween: " + DistanceBetweenHands);


        // if (GestureSwipe() == true)
        // {
        //  Debug.Log("God damn- it´s a swipe gesture");
        //  }

    }


    bool GestureSwipe()
    {
        // if ((((HandPalmRoll < HandPalmRollMin && HandPalmRoll > HandPalmRollMax ) == true) & ((HandPalmYaw < HandPalmYawMin && HandPalmYaw > HandPalmYawMax) == true) & ((pinch > PinchMax) == true)))
        if(HandPalmRoll < HandPalmRollMin && HandPalmRoll > HandPalmRollMax)
        {
            return true;
        }
        else return false;
    }

}