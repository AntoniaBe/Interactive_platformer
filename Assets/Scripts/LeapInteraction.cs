using UnityEngine;
using System.Collections;

public class LeapInteraction : MonoBehaviour {

    public void PauseGame() {
        GameController.instance.PauseGame();
    }

}
