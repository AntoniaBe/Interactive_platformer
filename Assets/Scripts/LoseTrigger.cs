using UnityEngine;
using System.Collections;

public class LoseTrigger : MonoBehaviour {

    public bool shouldKill = true;
    public bool shouldFallToDeath;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if(shouldFallToDeath) {
                other.GetComponent<NPC>().FallToDeath();
            } else if (shouldKill) {
                other.GetComponent<NPC>().Die();
            } else {
                GameController.instance.GameOver();
            }
        }
    }

}
