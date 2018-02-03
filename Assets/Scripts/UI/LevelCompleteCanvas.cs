using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for the victory screen.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class LevelCompleteCanvas : MonoBehaviour {

    /// <summary>
    /// The time until the stars start appearing.
    /// </summary>
    public float starInitialDelay = 2.5f;

    /// <summary>
    /// The delay between each indidvidual star appearing.
    /// </summary>
    public float starDelay = 0.5f;

    /// <summary>
    /// The delay before the jingle sound is played after stars appeared.
    /// </summary>
    public float jingleDelay = 0.5f;

    /// <summary>
    /// The time stars take to fade in.
    /// </summary>
    public float starFadeDuration = 0.25f;

    /// <summary>
    /// The time taken to darken the screen when it appears.
    /// </summary>
    public float darkenTime = 1f;

    /// <summary>
    /// The amount of time before the text and star outlines are faded in.
    /// </summary>
    public float fadeInDelay = 1f;

    /// <summary>
    /// The time taken until the text and star outlines finish fading in.
    /// </summary>
    public float fadeInTime = 1f;

    /// <summary>
    /// The time taken until the screen finishes fading to black.
    /// </summary>
    public float fadeOutTime = 1f;

    public RectTransform container;
    public Text title;
    public Text levelTime;
    public Image[] stars;
    public AudioClip[] victorySounds;
    public AudioClip wooshSound;
    public GameObject starParticlePrefab;
    public Image blackScreen;

    private AudioSource audioSource;
    private bool initAnimationDone;

    private void Start() {
        audioSource = GetComponent<AudioSource>();

        // Automatically set the correct camera - canvas needs to be screen space for the Particle System.
        GetComponent<Canvas>().worldCamera = Camera.main;

        // Start off with an alpha of zero - need to use the canvasRenderer for CrossFadeAlpha below to work
        title.canvasRenderer.SetAlpha(0f);
        levelTime.canvasRenderer.SetAlpha(0f);
        blackScreen.canvasRenderer.SetAlpha(0f);
        container.GetComponent<CanvasGroup>().alpha = 0f;
        foreach (var star in stars) {
            star.canvasRenderer.SetAlpha(0f);
        }

        // Fade in black screen and level time text
        blackScreen.enabled = true;
        blackScreen.CrossFadeAlpha(0.5f, darkenTime, true);

        levelTime.text = GameController.instance.LevelTimer.ToString("00.00") + " seconds";
        levelTime.CrossFadeAlpha(1f, darkenTime, true);

        // Fade in the star outlines and have the stars appear
        StartCoroutine(ShowStars());
    }

    private IEnumerator ShowStars() {
        yield return new WaitForSeconds(fadeInDelay);

        // Fade in the star outlines
        var canvasGroup = container.GetComponent<CanvasGroup>();
        float timer = 0f;
        while (timer < starInitialDelay) {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / starInitialDelay);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        // Swoosh in the stars individually
        int starCount = GameController.instance.CurrentStarCount;
        for (int i = 0; i < stars.Length; i++) {
            if (i < starCount) {
                audioSource.PlayOneShot(wooshSound);
                stars[i].CrossFadeAlpha(1f, starFadeDuration, true);
                Instantiate(starParticlePrefab, stars[i].transform);
            }
            yield return new WaitForSeconds(starDelay);
        }

        yield return new WaitForSeconds(jingleDelay);

        // Play victory jingle and fade in title
        title.CrossFadeAlpha(1f, 2f, true);
        audioSource.PlayOneShot(victorySounds[starCount - 1]);

        initAnimationDone = true;
    }

    public IEnumerator NextLevelAnimation() {
        // Wait for the appearance animation to finish
        while (!initAnimationDone) {
            yield return null;
        }

        // Fade the screen to dark completely
        blackScreen.CrossFadeAlpha(1f, fadeOutTime, true);

        // Shoot the screen content off the screen to the left
        var target = container.transform.position - new Vector3(container.sizeDelta.x, 0, 0);
        var velocity = Vector3.zero;
        float timer = 0f;
        while (timer < fadeOutTime) {
            container.transform.position = Vector3.SmoothDamp(container.transform.position, target, ref velocity, fadeOutTime, float.MaxValue, Time.unscaledDeltaTime);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
    }

}
