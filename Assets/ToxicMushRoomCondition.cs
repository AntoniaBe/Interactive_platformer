using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicMushRoomCondition : MonoBehaviour {

    public SnappingController[] snappingTriggers;
    public new ParticleSystem particleSystem;

    private void Update() {
        foreach (var trigger in snappingTriggers) {
            if (!trigger.HasSnapped) {
                break;
            }

            particleSystem.Clear();
            enabled = false;
        }
    }


    private void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player")) {
            collider.GetComponent<exAI>().Die();
        }
    }

}
