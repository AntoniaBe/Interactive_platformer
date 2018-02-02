using UnityEngine;
using System.Collections;

/// <summary>
/// Checks whether the castle level final gate has been opened and triggers a victory.
/// </summary>
public class CastleVictory : MonoBehaviour {

    public RisingGate gate;
    public GameObject skullSpawner;
    public GateWheelRotator gateWheel;

    private bool hasWon;

    private void Update() {
        if (!hasWon && gate.normalizedValue >= 1f) {
            // Pause all skulls and the skull spawner
            skullSpawner.SetActive(false);
            var skulls = FindObjectsOfType<FlyingSkull>();
            foreach(var skull in skulls) {
                skull.enabled = false;
            }

            // Lock the gate in place
            gateWheel.isLocked = true;

            // Display the victory animation on the player and show the victory screen
            StartCoroutine(VictoryAnimation());
            GameController.instance.Victory();
            hasWon = true;
        }
    }

    private IEnumerator VictoryAnimation() {
        const float turnTime = 0.5f;
        float timer = 0f;
        float angle = 0f;

        // Smoothly turn the player towards the gate.
        while (timer < turnTime) {
            angle = Mathf.LerpAngle(angle, 0f, timer / turnTime);
            transform.localEulerAngles = new Vector3(0f, angle, 0f);
            timer += Time.deltaTime;
            yield return null;
        }
    }

}
