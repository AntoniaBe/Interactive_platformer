using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public float levelTimer;

    private bool isLevelRunning;

    private void Awake() {
        instance = this;

        StartLevel();
    }

    private void Update() {
        if (isLevelRunning) {
            levelTimer += Time.deltaTime;
        }
    }

    public void StartLevel() {
        levelTimer = 0f;
        isLevelRunning = true;
    }

    public void StopLevel() {
        isLevelRunning = false;
    }

}
