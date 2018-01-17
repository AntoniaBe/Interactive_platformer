using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class GameOverCanvas : MonoBehaviour {

    public float fadeToGrayTime = 1f;
    public float fadeInBackgroundTime = 1f;
    public float fadeInTextTime = 1f;
    public float fadeOutTime = 1f;

    public Image image;
    public Text text;
    public Image blackScreen;

    private PostProcessingBehaviour postProcessing;

    private void Start() {
        postProcessing = Camera.main.GetComponent<PostProcessingBehaviour>();
        postProcessing.profile = Instantiate(postProcessing.profile); // create a copy of the profile so we don't change the source asset

        image.canvasRenderer.SetAlpha(0f);
        text.canvasRenderer.SetAlpha(0f);

        StartCoroutine(GameOverAnimation());
    }

    private IEnumerator GameOverAnimation() {
        float timer = 0;

        while (timer < fadeToGrayTime) {
            timer += Time.unscaledDeltaTime;

            var progress = timer / fadeToGrayTime;
            var colorGrading = postProcessing.profile.colorGrading.settings;
            colorGrading.basic.saturation = Mathf.Lerp(1f, 0f, progress);
            colorGrading.basic.contrast = Mathf.Lerp(1f, 2f, progress);
            postProcessing.profile.colorGrading.settings = colorGrading;

            var vignette = postProcessing.profile.vignette.settings;
            vignette.intensity = Mathf.Lerp(0f, 0.5f, progress);
            postProcessing.profile.vignette.settings = vignette;

            yield return null;
        }

        image.CrossFadeAlpha(1f, fadeInBackgroundTime, true);
        text.CrossFadeAlpha(1f, fadeInTextTime, true);
    }

    public IEnumerator RestartLevelAnimation() {
        blackScreen.canvasRenderer.SetAlpha(0f);
        blackScreen.enabled = true;
        blackScreen.CrossFadeAlpha(1f, fadeOutTime, true);

        yield return new WaitForSeconds(fadeOutTime);
    }

}
