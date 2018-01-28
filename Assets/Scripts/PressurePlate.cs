using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour {

    public bool active;

    private void OnCollisionEnter(Collision collision) {
        active = true;
    }

    private void OnCollisionExit(Collision collision) {
        active = false;
    }

}
