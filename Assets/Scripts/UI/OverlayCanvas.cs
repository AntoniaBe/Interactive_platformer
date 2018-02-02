using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverlayCanvas : MonoBehaviour {

    public float fadeInTime = 1f;

    public TextMeshProUGUI timer;
    public Image blackScreen;
    public Image[] stars;

    private float lastLevelTimer = -1f;
    private int lastStarCount = 3;

    private void Start() {
        blackScreen.enabled = true;
        blackScreen.canvasRenderer.SetAlpha(1f);
        blackScreen.CrossFadeAlpha(0f, fadeInTime, true);
    }

    private void Update() {
        float levelTimer = GameController.instance.LevelTimer;
        if (lastLevelTimer < levelTimer) {
            timer.text = "<mspace=35.0>" + levelTimer.ToString("00.00") + "</mspace>";
            lastLevelTimer = levelTimer;
        }

        int starCount = GameController.instance.CurrentStarCount;
        if (lastStarCount > starCount) {
            for (int i = stars.Length - 1; i >= starCount; i--) {
                stars[i].CrossFadeAlpha(0f, 0.5f, true);
            }
            lastStarCount = starCount;
        }
    }

}
