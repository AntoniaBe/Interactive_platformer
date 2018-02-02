using UnityEngine;

/// <summary>
/// Causes the player to wait upon entering the area, until a flag has been set.
/// </summary>
public class WaitingArea : MonoBehaviour {

    /// <summary>
    /// Whether the player should keep waiting in the area, or is allowed to move again.
    /// </summary>
    public bool ShouldWait { get; set; } = true;

    private NPC ai;
    private bool isPlayerInside;

    private void Start() {
        ai = GameObject.FindGameObjectWithTag("Player").GetComponent<NPC>();
    }

    private void Update() {
        if (isPlayerInside) {
            // Control player animations if there is a change in state.
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
