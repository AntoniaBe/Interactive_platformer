using UnityEngine;
using System.Collections;

public class LoseTrigger : MonoBehaviour {

    public bool shouldKill = true;
    public bool shouldFallToDeath;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if(shouldFallToDeath) {
                other.GetComponent<exAI>().FallToDeath();
            } else if (shouldKill) {
                other.GetComponent<exAI>().Die();
            } else {
                GameController.instance.GameOver();
            }
        }
    }

}
