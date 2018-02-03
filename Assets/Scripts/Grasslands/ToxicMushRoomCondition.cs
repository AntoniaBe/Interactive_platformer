using UnityEngine;

/// <summary>
/// Kills players through toxic mushroom gas. Disables itself once its snapping triggers have been filled.
/// </summary>
public class ToxicMushRoomCondition : MonoBehaviour {

    /// <summary>
    /// The snapping triggers that have to be filled to disable the mushrooms.
    /// </summary>
    public SnappingController[] snappingTriggers;

    /// <summary>
    /// The particle system emitting the toxic gas.
    /// </summary>
    public new ParticleSystem particleSystem;

    private void Update() {
        // Disable this component once all of its snapping areas have been filled.
        foreach (var trigger in snappingTriggers) {
            if (!trigger.HasSnapped) {
                break;
            }

            particleSystem.Clear();
            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider collider) {
        // Kill the player if it touches the toxic mushrooms.
        if (collider.CompareTag("Player")) {
            collider.GetComponent<NPC>().Die();
        }
    }

}
