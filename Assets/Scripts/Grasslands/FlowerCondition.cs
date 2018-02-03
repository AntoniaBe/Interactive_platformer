using System.Collections;
using UnityEngine;

public class FlowerCondition : MonoBehaviour {

    public GameObject speechBubble1, speechBubble2, speechBubble3, snapInFlower, butterfly, firstPoint, flower, followingFlower, butterflyAnimator;
    private bool disableSpeechBubble;
    private float moveTowardsSpeed = 5f;

    private void Start() {
        speechBubble2.GetComponent<Renderer>().enabled = false;
        speechBubble3.GetComponent<Renderer>().enabled = false;
    }

    public void CompleteQuest() {
        StartCoroutine(DisableSpeechBubbles());
        StartCoroutine(ButterflyLeaveAnimation());

        butterfly.transform.localEulerAngles = new Vector3(butterfly.transform.localEulerAngles.x, -180f, butterfly.transform.localEulerAngles.z);
        flower.GetComponent<Collider>().enabled = false;
    }

    private IEnumerator ButterflyLeaveAnimation() {
        while (true) {
            butterfly.transform.position = Vector3.MoveTowards(butterfly.transform.position, firstPoint.transform.position, moveTowardsSpeed * Time.deltaTime);
            flower.transform.position = followingFlower.transform.position;
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
