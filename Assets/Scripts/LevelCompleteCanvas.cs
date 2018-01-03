using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class LevelCompleteCanvas : MonoBehaviour {

    public float starInitialDelay = 2.5f;
    public float starDelay = 0.5f;
    public float jingleDelay = 0.5f;
    public float starFadeDuration = 0.25f;
    public float darkenTime = 1f;
    public float fadeInDelay = 1f;
    public float fadeInTime = 1f;
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
        while(timer < starInitialDelay) {
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
    }

    public IEnumerator NextLevelAnimation() {
        // Fade the screen to dark completely
        blackScreen.CrossFadeAlpha(1f, fadeOutTime, true);

        // Shoot the screen content off the screen to the left
        var target = container.transform.position - new Vector3(container.sizeDelta.x, 0, 0);
        var velocity = Vector3.zero;
        while (Vector3.Distance(container.transform.position, target) > 0.1f) {
            container.transform.position = Vector3.SmoothDamp(container.transform.position, target, ref velocity, fadeOutTime, float.MaxValue, Time.unscaledDeltaTime);
            yield return null;
        }
    }

}
