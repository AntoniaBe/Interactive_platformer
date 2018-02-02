using UnityEngine;
using System.Collections;

/// <summary>
/// Spawns flying skulls at a specific interval.
/// </summary>
public class SkullSpawner : MonoBehaviour {

    /// <summary>
    /// The prefab to instantiate when spawning.
    /// </summary>
    public GameObject skullPrefab;

    /// <summary>
    /// The interval at which skulls are spawned at.
    /// </summary>
    public float interval = 1f;

    /// <summary>
    /// The range skulls are spawned within randomly.
    /// </summary>
    public Vector3 range;

    /// <summary>
    /// The minimum distance of new spawns from the player.
    /// </summary>
    public float safeSpace = 3f;

    /// <summary>
    /// Whether the spawner should be spawning skulls.
    /// </summary>
    public bool ShouldSpawn { get; set; }

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

            if (ShouldSpawn) {
                var spawnPos = transform.position + new Vector3(Random.value < 0.5f ? -range.x - safeSpace : range.x + safeSpace, Random.Range(safeSpace, range.y), Random.Range(-range.z, range.z));
                var skull = Instantiate(skullPrefab);
                skull.transform.position = spawnPos;
                skull.GetComponent<FlyingSkull>().autoTarget = player;
            }
        }
    }

}
