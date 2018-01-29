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
    public SaveState SaveState { get; private set; } = new SaveState();

    public GameObject levelCompleteCanvasPrefab;
    public GameObject gameOverCanvasPrefab;
    public GameObject pauseCanvasPrefab;

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
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;

        SaveState = SaveState.LoadFromFile();

        DontDestroyOnLoad(gameObject);
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
        } else if (Input.GetKeyDown(KeyCode.P)) {
            PauseGame();
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public AsyncOperation LoadLevel(int level) {
        currentLevel = level;
        SaveState.UnlockLevel(currentLevel);

        var async = SceneManager.LoadSceneAsync("Level" + level);
        async.allowSceneActivation = false;
        return async;
    }

    public AsyncOperation LoadLevelSelection() {
        var async = SceneManager.LoadSceneAsync("LevelSelection");
        async.allowSceneActivation = false;
        return async;
    }

    public void StartLevel() {
        UnpauseGame();

        levelProperties = FindObjectOfType<LevelProperties>();
        LevelTimer = 0f;
        levelState = LevelState.RUNNING;
    }

    public void PauseGame() {
        if (levelState != LevelState.RUNNING) {
            return;
        }

        levelState = LevelState.PAUSED;
        Time.timeScale = 0f;

        Instantiate(pauseCanvasPrefab);
    }

    public void UnpauseGame() {
        if (levelState != LevelState.PAUSED) {
            return;
        }

        levelState = LevelState.RUNNING;
        Time.timeScale = 1f;

        var canvas = FindObjectOfType<PauseMenuCanvas>();
        if (canvas) {
            Destroy(canvas.gameObject);
        }
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
            async = LoadLevelSelection();
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
