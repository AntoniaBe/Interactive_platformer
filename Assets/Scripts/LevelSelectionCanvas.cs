using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionCanvas : MonoBehaviour {

    public float fadeOutTime = 2f;

    private AsyncOperation asyncOperation;

    public void SelectLevel(int level) {
        var buttons = GetComponentsInChildren<Button>();
        foreach (var button in buttons) {
            button.enabled = false;
        }

        asyncOperation = SceneManager.LoadSceneAsync("Level" + level);
        asyncOperation.allowSceneActivation = false;

        StartCoroutine(HideScreen());
    }

    public IEnumerator HideScreen() {
        var canvasGroup = GetComponent<CanvasGroup>();

        while (canvasGroup.alpha > float.Epsilon) {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, fadeOutTime);
            yield return null;
        }

        asyncOperation.allowSceneActivation = true;
    }

}
