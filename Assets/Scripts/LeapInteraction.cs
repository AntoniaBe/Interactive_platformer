using UnityEngine;
using System.Collections;

public class LeapInteraction : MonoBehaviour {

    public void PauseGame() {
        GetComponent<ClickGesture>().WaitBeforeDetection(0.5f);
        GameController.instance.PauseGame();
    }

    public void HandleSwipe() {
        if (GameController.instance.State == GameController.LevelState.GAMEOVER) {
            GameController.instance.RestartLevel();
        } else if (GameController.instance.State == GameController.LevelState.VICTORY) {
            GameController.instance.NextLevel();
        }
    }

}
