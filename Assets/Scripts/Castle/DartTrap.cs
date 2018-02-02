using System.Collections;
using UnityEngine;

/// <summary>
/// Fires projectiles at a specific time interval.
/// </summary>
public class DartTrap : MonoBehaviour {

    /// <summary>
    /// The prefab to instantiate for the projectile.
    /// </summary>
    public GameObject dartPrefab;

    /// <summary>
    /// The minimum of the random offset range to be applied to the projectile position.
    /// </summary>
    public Vector3 minOffset;

    /// <summary>
    /// The maximum of the random offset range to be applied to the projectile position.
    /// </summary>
    public Vector3 maxOffset;

    /// <summary>
    /// The interval at which projectiles are fired.
    /// </summary>
    public float interval = 1f;

    /// <summary>
    /// Whether projectiles should be fired (e.g. because the player is in range).
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Whether the trap has been disabled (e.g. because it's been broken).
    /// </summary>
    public bool Disabled { get; set; }

    private GameObject player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(FireDarts());
    }

    private IEnumerator FireDarts() {
        while (true) {
            yield return new WaitForSecondsRealtime(interval);
            if (IsActive && !Disabled) {
                var dart = Instantiate(dartPrefab);
                var offset = new Vector3(Random.Range(minOffset.x, maxOffset.x), Random.Range(minOffset.y, maxOffset.y), Random.Range(minOffset.z, maxOffset.z));
                dart.transform.position = transform.position + offset;
                dart.GetComponent<DartProjectile>().autoTarget = player;
            }
        }
    }

}
