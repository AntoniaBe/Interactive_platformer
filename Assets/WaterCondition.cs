using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCondition : MonoBehaviour {

    private GameObject player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player")) {
            player.GetComponent<Collider>().enabled = false;
            GameController.instance.GameOver();
            StartCoroutine(DoABackflip());
        }
    }

    private IEnumerator DoABackflip() {
        while (true) {
            player.transform.Rotate(new Vector3(Random.value, Random.value, Random.value));
            yield return null;
        }
    }

}
