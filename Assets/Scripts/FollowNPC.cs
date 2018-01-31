using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FollowNPC : MonoBehaviour {

    public Transform target;
    public Vector3 offset = new Vector3(0f, 1.0f, -2.0f);
    public float smoothTime = 0.25f;
    public float swipeOffsetDistance = 10f;
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
        if (!isSwiped) {
            targetPos = target.position + offset;
        } else {
            targetPos.y = target.position.y + offset.y;

            if (Mathf.Abs(target.position.x - targetPos.x) < 1f) {
                isSwiped = false;
                if (npc) {
                    npc.speedBoost = 1f;
                }
            } else {
                if (npc) {
                    if (targetPos.x - offset.x > target.position.x) {
                        npc.speedBoost = Mathf.Clamp(1f + (targetPos.x - target.position.x) * swipeSpeedBoost, 1f, 4f);
                    } else {
                        npc.speedBoost = 1f;
                    }
                }
            }
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref smoothVelocity, smoothTime, float.MaxValue, Time.unscaledDeltaTime);
    }

    public void CameraSwipe(bool value) {
        int dirX = value ? 1 : -1;
        targetPos += new Vector3(dirX * swipeOffsetDistance, 0, 0);
        isSwiped = true;
    }

    public void ResetSwipe() {
        isSwiped = false;
        if (npc) {
            npc.speedBoost = 1f;
        }
    }

}
