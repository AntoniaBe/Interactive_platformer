using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicMushRoomCondition : MonoBehaviour {

    public GameObject woodbox1, woodbox2;
    public ParticleSystem particeSystem;


	void Update () {

        if (woodbox1.GetComponent<Grabbable>().isSnappedIn && woodbox2.GetComponent<Grabbable>().isSnappedIn)
        {
            particeSystem.Clear();

        }
    }


    private void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.tag == "Player")
        {
            this.gameObject.GetComponent<NPCDies>().NpcDies();

        }

    }

}
