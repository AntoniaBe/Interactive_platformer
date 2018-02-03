using System.Collections;
using UnityEngine;

public class RoseThornCondition : MonoBehaviour {

    public GameObject flames;
    public GameObject torch;

    private void Start() {
        flames.SetActive(false);
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player")) {
            collider.GetComponent<NPC>().Die();
        } else if (collider.gameObject.name == "Torch") {
            flames.SetActive(true);
            Destroy(torch);
            StartCoroutine(WaitAndDeactivate());
        }
    }

    private IEnumerator WaitAndDeactivate() {
        yield return new WaitForSeconds(1.5f);
        flames.SetActive(false);
        gameObject.SetActive(false);
    }

}
