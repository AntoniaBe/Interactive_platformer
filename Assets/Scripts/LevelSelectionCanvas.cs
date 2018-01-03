using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionCanvas : MonoBehaviour {

    public float fadeOutTime = 2f;

    public void SelectLevel(int level) {
        var buttons = GetComponentsInChildren<Button>();
        foreach (var button in buttons) {
            button.enabled = false;
        }

        StartCoroutine(SelectLevelAnimation(level));
    }

    private IEnumerator SelectLevelAnimation(int level) {
        var async = GameController.instance.LoadLevel(level);

        var canvasGroup = GetComponent<CanvasGroup>();

        float timer = 0f;
        while (timer < fadeOutTime) {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeOutTime);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        async.allowSceneActivation = true;

        GameController.instance.StartLevel();
    }

}
