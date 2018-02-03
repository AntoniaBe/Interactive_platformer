using UnityEngine;

/// <summary>
/// Makes the containing object follow an object. Used both for the camera as well as the leap hands.
/// </summary>
public class FollowNPC : MonoBehaviour {

    /// <summary>
    /// The object to follow.
    /// </summary>
    public Transform target;

    /// <summary>
    /// The offset to be applied to the target position.
    /// </summary>
    public Vector3 offset = new Vector3(0f, 1.0f, -2.0f);

    /// <summary>
    /// The smoothing applied to the movement.
    /// </summary>
    public float smoothTime = 0.25f;

    /// <summary>
    /// The distance applied on a camera swipe.
    /// </summary>
    public float swipeOffsetDistance = 10f;

    /// <summary>
    /// The speed boost applied to the NPC on a camera swipe.
    /// </summary>
    public float swipeSpeedBoost = 0.1f;

    private Vector3 targetPos;
    private bool isSwiped;
    private Vector3 smoothVelocity;
    private NPC npc;

    private void Start() {
        npc = target.GetComponent<NPC>();
        targetPos = target.position + offset;
        transform.position = targetPos;
    }

    void LateUpdate() {
        // If we're not swiped, just follow the target as normal
        if (!isSwiped) {
            targetPos = target.position + offset;
        } else {
            // If we've swiped, still keep the y position up to date with the target
            targetPos.y = target.position.y + offset.y;

            // Cancel the swipe if the target gets close enough again
            if (Mathf.Abs(target.position.x - targetPos.x) < 1f) {
                isSwiped = false;
                if (npc) {
                    npc.speedBoost = 1f;
                }
            } else {
                // Apply a speedboost to the NPC based on the distance
                if (npc) {
                    if (targetPos.x - offset.x > target.position.x) {
                        npc.speedBoost = Mathf.Clamp(1f + (targetPos.x - target.position.x) * swipeSpeedBoost, 1f, 4f);
                    } else {
                        npc.speedBoost = 1f;
                    }
                }
            }
        }

        // Smoothly move this object towards the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref smoothVelocity, smoothTime, float.MaxValue, Time.unscaledDeltaTime);
    }

    /// <summary>
    /// Swipes the camera in the given direction.
    /// </summary>
    /// <param name="value">true when swiping right</param>
    public void CameraSwipe(bool value) {
        int dirX = value ? 1 : -1;
        targetPos += new Vector3(dirX * swipeOffsetDistance, 0, 0);
        isSwiped = true;
    }

    /// <summary>
    /// Resets the camera swipe, restoring normal target following.
    /// </summary>
    public void ResetSwipe() {
        isSwiped = false;
        if (npc) {
            npc.speedBoost = 1f;
        }
    }

}
