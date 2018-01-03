using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public enum LevelState {
        RUNNING,
        PAUSED,
        VICTORY,
        GAMEOVER
    }

    public static GameController instance;

    public GameObject levelCompleteCanvasPrefab;
    public GameObject gameOverCanvasPrefab;

    public float LevelTimer {
        get; private set;
    }

    public int CurrentStarCount {
        get {
            return 3;
        }
    }

    private LevelState levelState;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        StartLevel();
    }

    private void Update() {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.W)) {
            Victory();
        } else if (Input.GetKeyDown(KeyCode.L)) {
            GameOver();
        }
#endif

        if (Input.GetKeyDown(KeyCode.Return)) {
            if (levelState == LevelState.GAMEOVER) {
                StartCoroutine(RestartLevel());
            } else if (levelState == LevelState.VICTORY) {
                StartCoroutine(NextLevel());
            }
        }

        if (levelState == LevelState.RUNNING) {
            LevelTimer += Time.deltaTime;
        }
    }

    public void StartLevel() {
        LevelTimer = 0f;
        levelState = LevelState.RUNNING;
    }

    public void Victory() {
        levelState = LevelState.VICTORY;

        Instantiate(levelCompleteCanvasPrefab);
    }

    public void GameOver() {
        levelState = LevelState.GAMEOVER;

        Instantiate(gameOverCanvasPrefab);
    }

    private IEnumerator NextLevel() {
        var async = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        async.allowSceneActivation = false;

        var canvas = FindObjectOfType<LevelCompleteCanvas>();
        yield return canvas.StartCoroutine(canvas.NextLevelAnimation());

        async.allowSceneActivation = true;
    }

    private IEnumerator RestartLevel() {
        var async = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        async.allowSceneActivation = false;

        var canvas = FindObjectOfType<GameOverCanvas>();
        yield return canvas.StartCoroutine(canvas.RestartLevelAnimation());

        async.allowSceneActivation = true;
    }

}
