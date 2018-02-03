using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Causes a game over once the player enters the trigger area.
/// </summary>
public class LoseTrigger : MonoBehaviour {

    public UnityEvent onEnterArea;

    /// <summary>
    /// Whether the death animation should be played on the player.
    /// </summary>
    public bool shouldKill = true;

    /// <summary>
    /// Whether the falling animation should be played on the player. Has priority over the death animation.
    /// </summary>
    public bool shouldFallToDeath;

    private void Awake() {
        if (onEnterArea == null) {
            onEnterArea = new UnityEvent();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            // Invoke event for level-specific things to run
            onEnterArea.Invoke();

            if (shouldFallToDeath) {
                other.GetComponent<NPC>().FallToDeath();
            } else if (shouldKill) {
                other.GetComponent<NPC>().Die();
            } else {
                // This is usually done by the NPC itself, but for instant game-overs with no death animation we need to do it manually.
                GameController.instance.GameOver();
            }
        }
    }

}
