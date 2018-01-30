using UnityEngine;
using System.Collections;

public class WaitingArea : MonoBehaviour {

    public bool ShouldWait { get; set; } = true;

    private exAI ai;
    private bool isPlayerInside;

    private void Start() {
        ai = GameObject.FindGameObjectWithTag("Player").GetComponent<exAI>();
    }

    private void Update() {
        if (isPlayerInside) {
            if (ai.shouldWait && !ShouldWait) {
                ai.GetComponent<Animation>().Play();
            } else if (!ai.shouldWait && ShouldWait) {
                ai.GetComponent<Animation>().Stop();
            }
            ai.shouldWait = ShouldWait;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            isPlayerInside = false;
        }
    }

}
