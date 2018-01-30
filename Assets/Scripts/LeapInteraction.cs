using UnityEngine;
using System.Collections;

public class LeapInteraction : MonoBehaviour {

    public void PauseGame() {
        GetComponent<ClickGesture>().WaitBeforeDetection(0.5f);
        GameController.instance.PauseGame();
    }

}
