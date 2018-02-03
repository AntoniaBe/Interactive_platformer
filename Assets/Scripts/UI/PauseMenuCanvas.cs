using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Controller for the pause menu overlay.
/// </summary>
public class PauseMenuCanvas : MonoBehaviour {

    public Image blackScreen;

    /// <summary>
    /// The amount of time taken to fade the screen into black when exiting.
    /// </summary>
    public float exitLevelTime = 1f;

    private void Start() {
        // Automatically set the correct camera. The canvas needs to be on screen-space for the hands to appear in front.
        GetComponent<Canvas>().worldCamera = Camera.main;

        blackScreen.canvasRenderer.SetAlpha(0f);
    }

    /// <summary>
    /// Called by the continue button. Unpauses the game, effectively hiding this menu.
    /// </summary>
    public void Continue() {
        GameController.instance.UnpauseGame();
    }

    /// <summary>
    /// Called by the restart button. Disables all buttons to prevent double clicks and starts the level restarting process.
    /// </summary>
    public void RestartLevel() {
        DisableMenuButtons();

        GameController.instance.RestartLevel();
    }

    /// <summary>
    /// Called by the exit button. Disables all buttons to prevent double clicks and starts the transition to the level selection.
    /// </summary>
    public void ExitLevel() {
        DisableMenuButtons();

        StartCoroutine(ExitLevelAnimation());
    }

    private IEnumerator ExitLevelAnimation() {
        // Fade the screen to black while loading the level selection
        blackScreen.enabled = true;
        blackScreen.CrossFadeAlpha(1f, exitLevelTime, true);

        var async = GameController.instance.LoadLevelSelection();

        // Wait for the fade to finish before switching scenes
        yield return new WaitForSecondsRealtime(exitLevelTime);
        async.allowSceneActivation = true;
    }

    private void DisableMenuButtons() {
        foreach (var button in GetComponentsInChildren<Button>()) {
            button.enabled = false;
        }
    }

}
