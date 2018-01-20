using UnityEngine;

public class Grabbable : MonoBehaviour {

    public bool isTouchingHand;
    public bool isSnappedIn;

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag("hands")) {
            isTouchingHand = true;
            ApplyMaterialColor(Color.green);
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
    }


    private void OnTriggerExit(Collider collider) {
        if (collider.gameObject.CompareTag("hands")) {
            isTouchingHand = false;
            ApplyMaterialColor(Color.white);
        }
    }

    private void ApplyMaterialColor(Color color) {
        GetComponent<Renderer>().material.color = color;
        foreach (var renderer in GetComponentsInChildren<Renderer>()) {
            renderer.material.color = color;
        }
    }
}
