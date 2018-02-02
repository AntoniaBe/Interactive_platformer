using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenuCanvas : MonoBehaviour {

    public Image blackScreen;

    public float exitLevelTime = 1f;

    private void Start() {
        // Automatically set the correct camera. The canvas needs to be on screen-space for the hands to appear in front.
        GetComponent<Canvas>().worldCamera = Camera.main;

        blackScreen.canvasRenderer.SetAlpha(0f);
    }

    public void Continue() {
        GameController.instance.UnpauseGame();
    }

    public void RestartLevel() {
        DisableMenuButtons();

        GameController.instance.RestartLevel();
    }

    public void ExitLevel() {
        DisableMenuButtons();

        StartCoroutine(ExitLevelAnimation());
    }

    private IEnumerator ExitLevelAnimation() {
        blackScreen.enabled = true;
        blackScreen.CrossFadeAlpha(1f, exitLevelTime, true);

        var async = GameController.instance.LoadLevelSelection();

        yield return new WaitForSecondsRealtime(exitLevelTime);

        async.allowSceneActivation = true;
    }

    private void DisableMenuButtons() {
        foreach (var button in GetComponentsInChildren<Button>()) {
            button.enabled = false;
        }
    }

}
