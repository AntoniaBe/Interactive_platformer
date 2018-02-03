using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button controller to be used in the level selection screen.
/// </summary>
public class LevelSelectionEntry : MonoBehaviour {

    public GameObject starContainer;
    public Text bestTimeText;
    public Image[] stars;

    /// <summary>
    /// The id of the level to load on this button.
    /// </summary>
    public int level;

    private void Start() {
        // Setup the best time text and star display for this level
        var record = GameController.instance.SaveState.GetLevelRecord(level);
        bestTimeText.text = record != null ? record.bestTime.ToString("00.00") : "--.--";
        for (int i = 0; i < stars.Length; i++) {
            stars[i].enabled = record != null ? i < record.stars : false;
        }
    }

    /// <summary>
    /// Called when the button has been clicked.
    /// </summary>
    public void OnSelected() {
        GetComponentInParent<LevelSelectionCanvas>().SelectLevel(level);
    }

}
