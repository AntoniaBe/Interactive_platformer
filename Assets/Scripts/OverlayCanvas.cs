using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayCanvas : MonoBehaviour {

    public float fadeInTime = 1f;

    public Text timer;
    public Image blackScreen;

    private void Start() {
        blackScreen.enabled = true;
        blackScreen.canvasRenderer.SetAlpha(1f);
        blackScreen.CrossFadeAlpha(0f, fadeInTime, true);
    }

    private void Update() {
        timer.text = GameController.instance.levelTimer.ToString("00.00");
    }

}
