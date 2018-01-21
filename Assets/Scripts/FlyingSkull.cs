using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSkull : MonoBehaviour {

    private GameObject player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update() {

    }

}
