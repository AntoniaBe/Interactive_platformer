using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class LevelCompleteCanvas : MonoBehaviour {

    [Range(1, 3)]
    public int starCount = 3;

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
    public Image[] stars;
    public AudioClip[] victorySounds;
    public AudioClip wooshSound;
    public GameObject starParticlePrefab;
    public Image blackScreen;

    private AudioSource audioSource;

    private void Start() {
        GetComponent<Canvas>().worldCamera = Camera.main;

        audioSource = GetComponent<AudioSource>();

        title.canvasRenderer.SetAlpha(0f);
        blackScreen.canvasRenderer.SetAlpha(0f);
        container.GetComponent<CanvasGroup>().alpha = 0f;

        foreach (var star in stars) {
            star.canvasRenderer.SetAlpha(0f);
        }

        blackScreen.enabled = true;
        blackScreen.CrossFadeAlpha(0.5f, darkenTime, true);

        StartCoroutine(FadeIn());
        StartCoroutine(ShowStars(starCount));
    }

    public IEnumerator FadeIn() {
        yield return new WaitForSeconds(fadeInDelay);

        var canvasGroup = container.GetComponent<CanvasGroup>();

        var velocity = 0f;
        while(canvasGroup.alpha < 1f - float.Epsilon) {
            canvasGroup.alpha = Mathf.SmoothDamp(canvasGroup.alpha, 1f, ref velocity, fadeInTime);
            yield return null;
        }
    }

    public IEnumerator ShowStars(int starCount) {
        yield return new WaitForSeconds(starInitialDelay);

        for (int i = 0; i < stars.Length; i++) {
            if(i < starCount) {
                audioSource.PlayOneShot(wooshSound);
                stars[i].CrossFadeAlpha(1f, starFadeDuration, true);
                Instantiate(starParticlePrefab, stars[i].transform);
            }
            yield return new WaitForSeconds(starDelay);
        }

        yield return new WaitForSeconds(jingleDelay);

        title.CrossFadeAlpha(1f, 2f, true);

        audioSource.PlayOneShot(victorySounds[starCount - 1]);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            StartCoroutine(HideScreen());
        }
    }

    public IEnumerator HideScreen() {
        blackScreen.CrossFadeAlpha(1f, fadeOutTime, true);

        var target = container.transform.position - new Vector3(container.sizeDelta.x, 0, 0);
        var velocity = Vector3.zero;
        while (Vector3.Distance(container.transform.position, target) > 0.1f) {
            container.transform.position = Vector3.SmoothDamp(container.transform.position, target, ref velocity, fadeOutTime, float.MaxValue, Time.unscaledDeltaTime);
            yield return null;
        }
    }

}
