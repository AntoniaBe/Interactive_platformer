using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionCanvas : MonoBehaviour {

    public float fadeOutTime = 2f;

    private AsyncOperation asyncOperation;

    public void SelectLevel(int level) {
        var buttons = GetComponentsInChildren<Button>();
        foreach (var button in buttons) {
            button.enabled = false;
        }

        asyncOperation = GameController.instance.LoadLevel(level);

        StartCoroutine(HideScreen());
    }

    public IEnumerator HideScreen() {
        var canvasGroup = GetComponent<CanvasGroup>();

        float timer = 0f;
        while (canvasGroup.alpha > float.Epsilon) {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeOutTime);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        asyncOperation.allowSceneActivation = true;

        GameController.instance.StartLevel();
    }

}
