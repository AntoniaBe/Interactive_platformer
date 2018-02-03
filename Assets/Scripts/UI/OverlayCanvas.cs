using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controller for the overlay UI in a level.
/// </summary>
public class OverlayCanvas : MonoBehaviour {

    /// <summary>
    /// The time taken for the level screen to fade in from black.
    /// </summary>
    public float fadeInTime = 1f;

    public TextMeshProUGUI timer;
    public Image blackScreen;
    public Image[] stars;

    private float lastLevelTimer = -1f;
    private int lastStarCount = 3;

    private void Start() {
        // Fade out the black image covering the screen
        blackScreen.enabled = true;
        blackScreen.canvasRenderer.SetAlpha(1f);
        blackScreen.CrossFadeAlpha(0f, fadeInTime, true);
    }

    private void Update() {
        // Update the timer on screen if the timer has changed since the last frame
        float levelTimer = GameController.instance.LevelTimer;
        if (lastLevelTimer < levelTimer) {
            timer.text = "<mspace=35.0>" + levelTimer.ToString("00.00") + "</mspace>";
            lastLevelTimer = levelTimer;
        }

        // Update the stars displayed on screen if the amount has changed since the last frame
        int starCount = GameController.instance.CurrentStarCount;
        if (lastStarCount > starCount) {
            for (int i = stars.Length - 1; i >= starCount; i--) {
                stars[i].CrossFadeAlpha(0f, 0.5f, true);
            }
            lastStarCount = starCount;
        }
    }

}
