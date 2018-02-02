using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionEntry : MonoBehaviour {

    public GameObject starContainer;
    public Text bestTimeText;
    public Image[] stars;

    public int level;

    private void Start() {
        var button = GetComponentInChildren<Button>();
        var record = GameController.instance.SaveState.GetLevelRecord(level);
        bestTimeText.text = record != null ? record.bestTime.ToString("00.00") : "--.--";
        for (int i = 0; i < stars.Length; i++) {
            stars[i].enabled = record != null ? i < record.stars : false;
        }
    }

    public void OnSelected() {
        GetComponentInParent<LevelSelectionCanvas>().SelectLevel(level);
    }

}
