using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoseThornCondition : MonoBehaviour
{
    public bool collisionWithTorch = false;
    private GameObject flames, roseThorn, torch;

    private void Awake()
    {
        flames = GameObject.Find("Flamesc");
        roseThorn = GameObject.Find("rose-thorns");
        torch = GameObject.Find("Torch");
    }

    void Start()
    {
        collisionWithTorch = false;
        flames.SetActive(false);
    }

    void Update()
    {
        if (collisionWithTorch)
        {
            flames.SetActive(true);
            Destroy(torch);
            StartCoroutine(WaitAndDeactivitate());

        }
        else
        {
            flames.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.GetComponent<NPC>().Die();
        }

        if (collider.gameObject.name == "Torch")
        {
            collisionWithTorch = true;
        }
    }

    IEnumerator WaitAndDeactivitate()
    {
        yield return new WaitForSeconds(1.5f);
        flames.SetActive(false);
        roseThorn.SetActive(false);
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

}
