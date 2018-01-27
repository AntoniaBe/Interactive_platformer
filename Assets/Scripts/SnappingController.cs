using System.Collections;
using System.Linq;
using UnityEngine;

public class SnappingController : MonoBehaviour {

    public GameObject[] grabbables;

    private IEnumerator OnTriggerEnter(Collider col) {
        if (grabbables.Contains(col.gameObject)) {
            col.transform.position = transform.position;
            col.transform.rotation = transform.rotation;
            col.gameObject.layer = LayerMask.NameToLayer("SnapIn");
            col.GetComponent<Grabbable>().isSnappedIn = true;
            //col.isTrigger = false;
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }

}
