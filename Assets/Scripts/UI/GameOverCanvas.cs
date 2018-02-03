using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

/// <summary>
/// Controller for the game over screen.
/// </summary>
public class GameOverCanvas : MonoBehaviour {

    /// <summary>
    /// The time it takes to fade into gray when the screen appears.
    /// </summary>
    public float fadeToGrayTime = 1f;

    /// <summary>
    /// The time it takes for the black bar to finish fading in.
    /// </summary>
    public float fadeInBackgroundTime = 1f;

    /// <summary>
    /// The time it takes for the game over text to finish fading in.
    /// </summary>
    public float fadeInTextTime = 1f;

    /// <summary>
    /// The time it takes to fade the screen to black.
    /// </summary>
    public float fadeOutTime = 1f;

    public Image image;
    public Text text;
    public Image blackScreen;

    private PostProcessingBehaviour postProcessing;
    private bool initAnimationDone;

    private void Start() {
        postProcessing = Camera.main.GetComponent<PostProcessingBehaviour>();
        postProcessing.profile = Instantiate(postProcessing.profile); // create a copy of the profile so we don't change the source asset

        image.canvasRenderer.SetAlpha(0f);
        text.canvasRenderer.SetAlpha(0f);

        StartCoroutine(GameOverAnimation());
    }

    /// <summary>
    /// Fades the screen into grayscale and applies a vignette effect.
    /// </summary>
    /// <returns></returns>
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

        initAnimationDone = true;
    }

    /// <summary>
    /// Fades out the screen into black.
    /// </summary>
    /// <returns>coroutine</returns>
    public IEnumerator RestartLevelAnimation() {
        // Wait for the appearance animation to finish
        while (!initAnimationDone) {
            yield return null;
        }

        blackScreen.canvasRenderer.SetAlpha(0f);
        blackScreen.enabled = true;
        blackScreen.CrossFadeAlpha(1f, fadeOutTime, true);

        yield return new WaitForSeconds(fadeOutTime);
    }

}
