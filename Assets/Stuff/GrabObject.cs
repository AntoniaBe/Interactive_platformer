using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;


public class GrabObject : MonoBehaviour {

    private LeapServiceProvider provider;

    // Use this for initialization
    void Start () {

        provider = FindObjectOfType<LeapServiceProvider>() as LeapServiceProvider;

    }

    // Update is called once per frame
    void Update () {


    }
}
