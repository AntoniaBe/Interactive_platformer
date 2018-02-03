using System.Collections;
using UnityEngine;

public class DeerFamilyCondition : MonoBehaviour {

    private Animator animatorDeer, animatorDoeAndKid;
    public GameObject deer, doe, kid;
    private float speed = 10f;
    private Vector3 endPositionAttack, endPositionFleeing;
    public bool handTouchingDeerFamily, npcCollides;

    private void FixedUpdate() {
        if (handTouchingDeerFamily) {
            TurnAndRun(doe, new Vector3(0, -2f, 0));
            TurnAndRun(kid, new Vector3(0, -2.8f, 0));
            TurnAndRun(deer, new Vector3(0, -1.518f, 0));
        }

        if (npcCollides) {
            DeerFamilyAttacks(doe, new Vector3(0, -91.483f, 0));
            DeerFamilyAttacks(deer, new Vector3(0, -90.408f, 0));
            DeerFamilyAttacks(kid, new Vector3(0, kid.transform.localEulerAngles.y, 0));
        }
    }

    private IEnumerator OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("hands")) {
            handTouchingDeerFamily = true;
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(false);
        } else if (collider.CompareTag("Player")) {
            npcCollides = true;
            yield return new WaitForSeconds(0.5f);
            collider.GetComponent<NPC>().Die();
            yield return new WaitForSeconds(1.5f);
            gameObject.SetActive(false);
        }
    }

    private void TurnAndRun(GameObject gameobject, Vector3 angle) {
        endPositionFleeing = new Vector3(gameobject.transform.position.x, gameobject.transform.position.y, 80f);
        gameobject.GetComponent<Animator>().SetBool("IsRun", true);
        gameobject.transform.localEulerAngles = angle;
        gameobject.transform.position = Vector3.MoveTowards(gameobject.transform.position, endPositionFleeing, speed * Time.deltaTime);
    }

    private void DeerFamilyAttacks(GameObject gameobject, Vector3 angle) {
        endPositionAttack = new Vector3(0f, gameobject.transform.position.y, 0f);
        gameobject.transform.localEulerAngles = angle;
        gameobject.GetComponent<Animator>().SetBool("IsRun", true);
        gameobject.transform.position = Vector3.MoveTowards(gameobject.transform.position, endPositionAttack, speed * Time.deltaTime);
    }

}
