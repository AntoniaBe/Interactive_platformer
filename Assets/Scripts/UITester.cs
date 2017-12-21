using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITester : MonoBehaviour {

    public GameObject levelCompleteCanvasPrefab;
    public GameObject gameOverCanvasPrefab;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            TestVictoryScreen(1);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            TestVictoryScreen(2);
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            TestVictoryScreen(3);
        } else if(Input.GetKeyDown(KeyCode.L)) {
            TestGameOverScreen();
        }
    }

    private void TestVictoryScreen(int stars) {
        var screenObj = Instantiate(levelCompleteCanvasPrefab);
        var screen = screenObj.GetComponent<LevelCompleteCanvas>();
        screen.starCount = stars;
    }

    private void TestGameOverScreen() {
        var screenObj = Instantiate(gameOverCanvasPrefab);
        var screen = screenObj.GetComponent<GameOverCanvas>();
    }

}
