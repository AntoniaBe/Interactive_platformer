using System.Collections;
using UnityEngine;

public class DartTrap : MonoBehaviour {

    public GameObject player;
    public GameObject dartPrefab;
    public Vector3 minOffset;
    public Vector3 maxOffset;
    public float interval = 1f;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(FireDarts());
    }

    private IEnumerator FireDarts() {
        while (true) {
            yield return new WaitForSecondsRealtime(interval);
            var dart = Instantiate(dartPrefab);
            var offset = new Vector3(Random.Range(minOffset.x, maxOffset.x), Random.Range(minOffset.y, maxOffset.y), Random.Range(minOffset.z, maxOffset.z));
            dart.transform.position = transform.position + offset;
            dart.GetComponent<DartProjectile>().autoTarget = player;
        }
    }

}
