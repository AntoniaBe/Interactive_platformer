using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for the level selection screen.
/// </summary>
public class LevelSelectionCanvas : MonoBehaviour {

    /// <summary>
    /// The time taken to fade the screen to black.
    /// </summary>
    public float fadeOutTime = 2f;

    /// <summary>
    /// Called by a level selection button. Buttons will be disabled and the screen will transition into the selected level.
    /// </summary>
    /// <param name="level"></param>
    public void SelectLevel(int level) {
        // Disable all buttons to prevent double loads
        var buttons = GetComponentsInChildren<Button>();
        foreach (var button in buttons) {
            button.enabled = false;
        }

        // Start the level loading operation
        StartCoroutine(SelectLevelAnimation(level));
    }

    private IEnumerator SelectLevelAnimation(int level) {
        var async = GameController.instance.LoadLevel(level);
        var canvasGroup = GetComponent<CanvasGroup>();

        // Fade out the screen into black
        float timer = 0f;
        while (timer < fadeOutTime) {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeOutTime);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        // Allow the loaded scene to activate and start the level
        async.allowSceneActivation = true;
        GameController.instance.StartLevel();
    }

}
