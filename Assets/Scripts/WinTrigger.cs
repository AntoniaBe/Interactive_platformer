using UnityEngine;
using System.Collections;

public class WinTrigger : MonoBehaviour {

    private GameObject player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            player.GetComponent<exAI>().shouldWait = true;
            GameController.instance.Victory();
        }
    }

}
