using UnityEngine;
using System.Collections;

public class LoseTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            GameController.instance.GameOver();
        }
    }

}
