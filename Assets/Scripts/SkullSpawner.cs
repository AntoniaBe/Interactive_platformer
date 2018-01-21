using UnityEngine;
using System.Collections;

public class SkullSpawner : MonoBehaviour {

    public GameObject skullPrefab;
    public float interval = 1f;
    public Vector3 range;
    public float minPlayerDist = 2f;
    public float safeSpace = 3f;

    private GameObject player;
    private new Camera camera;

    private void Start() {
        camera = Camera.main;

        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(SpawnSkulls());
    }

    private IEnumerator SpawnSkulls() {
        while (true) {
            yield return new WaitForSeconds(interval);

            if (Vector3.Distance(transform.position, player.transform.position) <= minPlayerDist) {
                var spawnPos = transform.position + new Vector3(Random.value < 0.5f ? -range.x - safeSpace : range.x + safeSpace, Random.Range(-range.y, range.y), Random.Range(-range.z, range.z));
                var skull = Instantiate(skullPrefab);
                skull.transform.position = spawnPos;
                skull.GetComponent<FlyingSkull>().autoTarget = player;
            }
        }
    }

}
