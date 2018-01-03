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
    public SaveState SaveState { get; private set; }

    public GameObject levelCompleteCanvasPrefab;
    public GameObject gameOverCanvasPrefab;

    public int maxLevels;

    private int currentLevel = 1;
    private LevelProperties levelProperties;

    public float LevelTimer {
        get; private set;
    }

    public int CurrentStarCount {
        get {
            if (!levelProperties) {
                return 3;
            }

            for (int i = levelProperties.levelTimes.Length - 1; i >= 0; i--) {
                if (LevelTimer < levelProperties.levelTimes[i]) {
                    return i + 1;
                }
            }

            return 1;
        }
    }

    private LevelState levelState;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        SaveState = SaveState.LoadFromFile();
    }

    private void Update() {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.W)) {
            Victory();
        } else if (Input.GetKeyDown(KeyCode.L)) {
            GameOver();
        } else if (Input.GetKeyDown(KeyCode.R)) {
            RestartLevel();
        } else if (Input.GetKeyDown(KeyCode.N)) {
            StartCoroutine(NextLevel());
        }
#endif

        if (Input.GetKeyDown(KeyCode.Return)) {
            if (levelState == LevelState.GAMEOVER) {
                RestartLevel();
            } else if (levelState == LevelState.VICTORY) {
                StartCoroutine(NextLevel());
            }
        }

        if (levelState == LevelState.RUNNING) {
            LevelTimer += Time.deltaTime;
        }
    }

    public AsyncOperation LoadLevel(int level) {
        currentLevel = level;
        SaveState.UnlockLevel(currentLevel);

        var asyncOperation = SceneManager.LoadSceneAsync("Level" + level);
        asyncOperation.allowSceneActivation = false;
        return asyncOperation;
    }

    public void StartLevel() {
        levelProperties = FindObjectOfType<LevelProperties>();
        LevelTimer = 0f;
        levelState = LevelState.RUNNING;
    }

    public void Victory() {
        levelState = LevelState.VICTORY;

        SaveState.UpdateLevelRecord(currentLevel, LevelTimer, CurrentStarCount);

        Instantiate(levelCompleteCanvasPrefab);
    }

    public void GameOver() {
        levelState = LevelState.GAMEOVER;

        Instantiate(gameOverCanvasPrefab);
    }

    private IEnumerator NextLevel() {
        AsyncOperation async;
        if (currentLevel + 1 >= maxLevels) {
            async = SceneManager.LoadSceneAsync("LevelSelection");
            async.allowSceneActivation = false;
        } else {
            async = LoadLevel(currentLevel + 1);
        }

        var canvas = FindObjectOfType<LevelCompleteCanvas>();
        if (canvas) {
            yield return canvas.StartCoroutine(canvas.NextLevelAnimation());
        }

        async.allowSceneActivation = true;

        StartLevel();
    }

    public void RestartLevel() {
        StartCoroutine(RestartLevelCoroutine());
    }

    private IEnumerator RestartLevelCoroutine() {
        var async = LoadLevel(currentLevel);

        var canvas = FindObjectOfType<GameOverCanvas>();
        if (canvas) {
            yield return canvas.StartCoroutine(canvas.RestartLevelAnimation());
        }

        async.allowSceneActivation = true;

        StartLevel();
    }

}
