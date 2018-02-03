using UnityEngine;

/// <summary>
/// Rises the bridge to the target position once all conditions are met.
/// </summary>
public class MoveBridge : MonoBehaviour {

    public bool SmallStatueDone { get; set; }
    public bool LargeStatueDone { get; set; }

    public GameObject bridgeTrigger;

    /// <summary>
    /// Move bridge to target position once the statue riddle has been solved.
    /// </summary>
    private void Update() {
        if (SmallStatueDone && LargeStatueDone) {
            if (transform.position.y < bridgeTrigger.transform.position.y) {
                transform.Translate(Vector3.up * Time.deltaTime, Space.World);
            }
        }
    }

}
