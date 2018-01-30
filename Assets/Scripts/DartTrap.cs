using System.Collections;
using UnityEngine;

public class DartTrap : MonoBehaviour {

    
    public GameObject dartPrefab;
    public Vector3 minOffset;
    public Vector3 maxOffset;
    public float interval = 1f;

    public bool IsActive { get; set; }

    private GameObject player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(FireDarts());
    }

    private IEnumerator FireDarts() {
        while (true) {
            yield return new WaitForSecondsRealtime(interval);
            if (IsActive) {
                var dart = Instantiate(dartPrefab);
                var offset = new Vector3(Random.Range(minOffset.x, maxOffset.x), Random.Range(minOffset.y, maxOffset.y), Random.Range(minOffset.z, maxOffset.z));
                dart.transform.position = transform.position + offset;
                dart.GetComponent<DartProjectile>().autoTarget = player;
            }
        }
    }

}
