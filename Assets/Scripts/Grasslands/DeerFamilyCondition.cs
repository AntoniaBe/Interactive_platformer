using System.Collections;
using UnityEngine;

/// <summary>
/// Makes the deer family attack the NPC or run away from the hands.
/// </summary>
public class DeerFamilyCondition : MonoBehaviour {

    /// <summary>
    /// The deer objects of the deer family.
    /// </summary>
    public GameObject deer, doe, kid;

    private float speed = 10f;
    private bool isAttacking;
    private bool isFleeing;

    private void Update() {
        // Move the family depending on whether they're attacking or fleeing
        if (isAttacking) {
            var targetVector = new Vector3(0f, deer.transform.position.y, deer.transform.position.z);
            var angleVector = new Vector3(0, -90f, 0);
            TurnAndRun(doe, targetVector, angleVector);
            TurnAndRun(deer, targetVector, angleVector);
            TurnAndRun(kid, targetVector, angleVector);
        } else if (isFleeing) {
            var targetVector = new Vector3(deer.transform.position.x, deer.transform.position.y, 80f);
            TurnAndRun(doe, targetVector, Vector3.zero);
            TurnAndRun(kid, targetVector, Vector3.zero);
            TurnAndRun(deer, targetVector, Vector3.zero);
        }
    }

    private void OnTriggerEnter(Collider collider) {
        // If touching a hand, run away. If touching the NPC, attack it!
        if (collider.CompareTag("hands")) {
            isFleeing = true;
            StartCoroutine(WaitAndDeactivate());
        } else if (collider.CompareTag("Player")) {
            isAttacking = true;
            StartCoroutine(WaitAndKillNPC(collider.GetComponent<NPC>()));
            StartCoroutine(WaitAndDeactivate());
        }
    }

    /// <summary>
    /// Rotates the deer object to the given angle and smoothly moves it towards the target position.
    /// </summary>
    /// <param name="deerObject">the deer object to move</param>
    /// <param name="target">the target position to move to</param>
    /// <param name="angle">the angle to rotate to</param>
    private void TurnAndRun(GameObject deerObject, Vector3 target, Vector3 angle) {
        deerObject.GetComponent<Animator>().SetBool("IsRun", true);
        deerObject.transform.localEulerAngles = angle;
        deerObject.transform.position = Vector3.MoveTowards(deerObject.transform.position, target, speed * Time.deltaTime);
    }

    /// <summary>
    /// Kills the NPC after a short delay.
    /// </summary>
    /// <param name="npc">the npc object</param>
    /// <returns>coroutine</returns>
    private IEnumerator WaitAndKillNPC(NPC npc) {
        yield return new WaitForSeconds(0.5f);
        npc.Die();
    }

    /// <summary>
    /// Deactivates the deer family after a short delay.
    /// </summary>
    /// <returns>coroutine</returns>
    private IEnumerator WaitAndDeactivate() {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

}
