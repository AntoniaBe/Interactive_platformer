using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerCondition : MonoBehaviour {


    public GameObject speechBubble1, speechBubble2, speechBubble3, snapInFlower, butterfly, firstPoint, flower, followingFlower, AI, makeNPCStop, butterflyAnimator;
    private bool disableSpeechBubble;
    private float moveTowardsSpeed = 5f;


    void Start () {
        speechBubble2.GetComponent<Renderer>().enabled = false;
        speechBubble3.GetComponent<Renderer>().enabled = false;

    }

	void Update () {

        if (snapInFlower.GetComponent<Grabbable>().IsSnappedIn)
        {
            StartCoroutine(DisableSpeechBubbles());
            makeNPCStop.SetActive(false);
            butterfly.transform.localEulerAngles = new Vector3(butterfly.transform.localEulerAngles.x, -180f, butterfly.transform.localEulerAngles.z);
            butterfly.transform.position = Vector3.MoveTowards(butterfly.transform.position, firstPoint.transform.position, moveTowardsSpeed * Time.deltaTime);
            flower.transform.position = followingFlower.transform.position;
            AI.GetComponent<Animation>().Play();
            AI.GetComponent<NPC>().speed = 2;
            flower.GetComponent<Collider>().enabled = false;

            if (butterfly.transform.position == firstPoint.transform.position)
            {
                butterfly.SetActive(false);
                flower.SetActive(false);
                followingFlower.SetActive(false);
            }

        }
    }


    private void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.name == "Flower-1-Red" || collider.gameObject.name == "Flower-1-Blue" | collider.gameObject.name == "Flower-1-Violet")
        {

            speechBubble1.GetComponent<Renderer>().enabled = false;
            speechBubble2.GetComponent<Renderer>().enabled = true;
        }

        if (collider.gameObject.name == "Flower-1-Yellow")
        {
            speechBubble1.GetComponent<Renderer>().enabled = false;
            speechBubble2.GetComponent<Renderer>().enabled = false;
            speechBubble3.GetComponent<Renderer>().enabled = true;
            disableSpeechBubble = true;
            
        }

    }

    IEnumerator DisableSpeechBubbles()
    {
        yield return new WaitForSeconds(2f);
        speechBubble1.SetActive(false);
        speechBubble2.SetActive(false);
        speechBubble3.SetActive(false);
    }


}
