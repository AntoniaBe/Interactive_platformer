using System.Collections;
using UnityEngine;

/// <summary>
/// Kills the player on touch. Can be burned by a torch.
/// </summary>
public class RoseThornCondition : MonoBehaviour {

    /// <summary>
    /// The flames particle effect to show on the bush when its lit.
    /// </summary>
    public GameObject flames;

    /// <summary>
    /// The torch used to light up the rose thorn bush.
    /// </summary>
    public GameObject torch;

    private void Start() {
        flames.SetActive(false);
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player")) {
            // Kill the player on touch.
            collider.GetComponent<NPC>().Die();
        } else if (collider.gameObject.name == "Torch") {
            // When touching the torch, go up in flames
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
