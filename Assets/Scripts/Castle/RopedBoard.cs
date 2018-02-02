using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopedBoard : MonoBehaviour {

    public GameObject board;
    public Rigidifier sword;

    private Rope[] ropes;
    private bool isBoardDetached;

    private void Start() {
        ropes = GetComponentsInChildren<Rope>();
    }

    private void Update() {
        if (!isBoardDetached) {
            CheckRopes();
        }
    }

    private void CheckRopes() {
        foreach (var rope in ropes) {
            if (!rope.isDetached) {
                return;
            }
        }

        isBoardDetached = true;
        sword.CanRigidify = true;
        var rb = board.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX;
        board.transform.parent = null;
    }

}
