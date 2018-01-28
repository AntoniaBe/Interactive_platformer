using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyCondition : MonoBehaviour
{

    public GameObject AI, speechBubble1;
    private bool speechBubble1Scale;


    void Start()
    {
        speechBubble1.transform.localScale = new Vector3(0, 0, 0);
    }


    void Update()
    {

        if (speechBubble1Scale)
        {
            ScaleBubble(speechBubble1);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.tag == "Player")
        {
            AI.GetComponent<Animation>().Stop();
            AI.GetComponent<exAI>().speed = 0;
            speechBubble1Scale = true;


        }

    }

    private void ScaleBubble(GameObject speechbubble)
    {
        float newScale = Mathf.Lerp(0, 1f, 4);
        speechbubble.transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}