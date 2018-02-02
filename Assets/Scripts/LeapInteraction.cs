using UnityEngine;

/// <summary>
/// Controller class mapping leap interaction from within scenes to the singleton GameController.
/// </summary>
public class LeapInteraction : MonoBehaviour {

    /// <summary>
    /// Opens the pause screen and applies a short cooldown to the click gesture to prevent accidental clicks.
    /// </summary>
    public void PauseGame() {
        GetComponent<ClickGesture>().WaitBeforeDetection(0.5f);
        GameController.instance.PauseGame();
    }

    /// <summary>
    /// If on the GameOver or Victory screen, calling this function will restart the current level, or proceed to the next level.
    /// </summary>
    public void HandleSwipe() {
        if (GameController.instance.State == GameController.LevelState.GAMEOVER) {
            GameController.instance.RestartLevel();
        } else if (GameController.instance.State == GameController.LevelState.VICTORY) {
            GameController.instance.NextLevel();
        }
    }

}
