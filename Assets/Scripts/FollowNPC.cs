using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FollowNPC : MonoBehaviour { // TODO rename to CameraController?

    public Transform target;
    public Vector3 offset = new Vector3(0f, 1.0f, -2.0f);
    public float swipeOffsetDistance = 10f;
    public float swipeTime = 0.25f;

    private Vector3 swipeOffset;
    private Vector3 swipeOffsetTarget;
    private Vector3 swipeOffsetVelocity;

    void LateUpdate() {
        swipeOffset = Vector3.SmoothDamp(swipeOffset, swipeOffsetTarget, ref swipeOffsetVelocity, swipeTime, float.MaxValue);

        transform.position = target.position + offset + swipeOffset;
    }

    public void CameraSwipe(bool value) {
        int dirX = value ? 1 : -1;
        swipeOffsetTarget += new Vector3(dirX * swipeOffsetDistance, 0, 0);
    } 

}
