using UnityEngine;

/// <summary>
/// Causes a victory once the player enters the trigger area.
/// </summary>
public class WinTrigger : MonoBehaviour {

    private GameObject player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            player.GetComponent<NPC>().shouldWait = true;
            GameController.instance.Victory();
        }
    }

}
