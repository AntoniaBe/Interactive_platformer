using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the butterfly speech bubbles and makes it fly away once the correct flower has been given.
/// </summary>
public class FlowerCondition : MonoBehaviour {

    /// <summary>
    /// The initial speech bubble shown.
    /// </summary>
    public GameObject speechBubble1;

    /// <summary>
    /// The speech bubble shown if a wrong flower is given.
    /// </summary>
    public GameObject speechBubble2;

    /// <summary>
    /// The speech bubble shown if the correct flower is given.
    /// </summary>
    public GameObject speechBubble3;

    /// <summary>
    /// The butterfly object.
    /// </summary>
    public GameObject butterfly;

    /// <summary>
    /// The point the butterfly flies towards when the quest is completed.
    /// </summary>
    public GameObject firstPoint;

    /// <summary>
    /// The yellow flower the butterfly wants.
    /// </summary>
    public GameObject flower;

    /// <summary>
    /// Target transform for the flower once the butterfly grabs it.
    /// </summary>
    public GameObject followingFlower;

    private bool disableSpeechBubble;
    private float moveTowardsSpeed = 5f;

    private void Start() {
        speechBubble2.GetComponent<Renderer>().enabled = false;
        speechBubble3.GetComponent<Renderer>().enabled = false;
    }

    /// <summary>
    /// Called by the snapping controller. Causes the butterfly to grab the flower and leave.
    /// </summary>
    public void CompleteQuest() {
        StartCoroutine(DisableSpeechBubbles());
        StartCoroutine(ButterflyLeaveAnimation());

        butterfly.transform.localEulerAngles = new Vector3(butterfly.transform.localEulerAngles.x, -180f, butterfly.transform.localEulerAngles.z);
        flower.transform.position = followingFlower.transform.position;
        flower.transform.parent = followingFlower.transform;
        flower.GetComponent<Collider>().enabled = false;
    }

    private IEnumerator ButterflyLeaveAnimation() {
        while (true) {
            butterfly.transform.position = Vector3.MoveTowards(butterfly.transform.position, firstPoint.transform.position, moveTowardsSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.name == "Flower-1-Red" || collider.gameObject.name == "Flower-1-Blue" | collider.gameObject.name == "Flower-1-Violet") {
            speechBubble1.GetComponent<Renderer>().enabled = false;
            speechBubble2.GetComponent<Renderer>().enabled = true;
        }

        if (collider.gameObject.name == "Flower-1-Yellow") {
            speechBubble1.GetComponent<Renderer>().enabled = false;
            speechBubble2.GetComponent<Renderer>().enabled = false;
            speechBubble3.GetComponent<Renderer>().enabled = true;
            disableSpeechBubble = true;

        }
    }

    private IEnumerator DisableSpeechBubbles() {
        yield return new WaitForSeconds(2f);
        speechBubble1.SetActive(false);
        speechBubble2.SetActive(false);
        speechBubble3.SetActive(false);
    }

}
