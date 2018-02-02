using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton class managing the game state and UI.
/// </summary>
public class GameController : MonoBehaviour {

    /// <summary>
    /// Represents the state the current level is in.
    /// </summary>
    public enum LevelState {
        RUNNING,
        PAUSED,
        VICTORY,
        GAMEOVER
    }

    /// <summary>
    /// Singleton instance of the GameController.
    /// </summary>
    public static GameController instance;

    /// <summary>
    /// The save state containing information on level best times and star achievements.
    /// </summary>
    public SaveState SaveState { get; private set; } = new SaveState();

    /// <summary>
    /// The prefab to instantiate for the victory screen.
    /// </summary>
    public GameObject levelCompleteCanvasPrefab;

    /// <summary>
    /// The prefab to instantiate for the game over screen.
    /// </summary>
    public GameObject gameOverCanvasPrefab;

    /// <summary>
    /// The prefab to instantiate for the pause menu.
    /// </summary>
    public GameObject pauseCanvasPrefab;

    /// <summary>
    /// The amount of levels implemented in the game. Attempting to enter a level id larger than this will return the player to the level selection.
    /// </summary>
    public int maxLevels;

    private int currentLevel = 1;
    private LevelProperties levelProperties;

    /// <summary>
    /// The time the player has spent in the current level since the last restart.
    /// </summary>
    public float LevelTimer {
        get; private set;
    }

    /// <summary>
    /// The amount of stars the player would unlock based on the current level timer.
    /// </summary>
    public int CurrentStarCount {
        get {
            // If for some reason there are no level properties in the scene, just assume three stars
            if (!levelProperties) {
                return 3;
            }

            // Find the best matching time based on the level properties
            for (int i = levelProperties.levelTimes.Length - 1; i >= 0; i--) {
                if (LevelTimer < levelProperties.levelTimes[i]) {
                    return i + 1;
                }
            }

            return 1;
        }
    }

    /// <summary>
    /// The state the game is currently in (e.g. PAUSED).
    /// </summary>
    public LevelState State { get; private set; }

    private void Awake() {
        // If this singleton already exists, destroy yourself
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        SaveState = SaveState.LoadFromFile();
    }

    private void Start() {
#if UNITY_EDITOR
        // When entering a level from the editor as opposed to the level selection, call StartLevel manually
        if (FindObjectOfType<LevelProperties>()) {
            StartLevel();
        }
#endif
    }

    private void Update() {
        // Continuously try to find the level properties object, until it's found
        // This is necessary because by the time StartLevel is called, the scene has not been fully activated yet
        if (!levelProperties) {
            levelProperties = FindObjectOfType<LevelProperties>();
        }

#if UNITY_EDITOR
        // Cheaty keys for quick testing
        if (Input.GetKeyDown(KeyCode.W)) {
            Victory();
        } else if (Input.GetKeyDown(KeyCode.L)) {
            GameOver();
        } else if (Input.GetKeyDown(KeyCode.R)) {
            RestartLevel();
        } else if (Input.GetKeyDown(KeyCode.N)) {
            NextLevel();
        } else if (Input.GetKeyDown(KeyCode.P)) {
            PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            if (State == LevelState.GAMEOVER) {
                RestartLevel();
            } else if (State == LevelState.VICTORY) {
                NextLevel();
            }
        }
#endif

        // If the game is currently running a level, increase the level timer
        if (State == LevelState.RUNNING) {
            LevelTimer += Time.deltaTime;
        }
    }

    /// <summary>
    /// Asynchronously loads the level with the given id. The loaded level will not activate on its own, it's up to the caller to do that when ready.
    /// </summary>
    /// <param name="level">the id of the level to load</param>
    /// <returns>the AsyncOperation of the level loading</returns>
    public AsyncOperation LoadLevel(int level) {
        currentLevel = level;

        var async = SceneManager.LoadSceneAsync("Level" + level);
        async.allowSceneActivation = false;
        return async;
    }

    /// <summary>
    /// Asynchronously loads the level selection screen. The screen will not activate on its own, it's up to the caller to do that when ready.
    /// </summary>
    /// <returns>the AsyncOperation of the scene loading</returns>
    public AsyncOperation LoadLevelSelection() {
        var async = SceneManager.LoadSceneAsync("LevelSelection");
        async.allowSceneActivation = false;
        return async;
    }

    /// <summary>
    /// Resets the level timer and puts the game into running state.
    /// </summary>
    public void StartLevel() {
        UnpauseGame();

        levelProperties = FindObjectOfType<LevelProperties>();
        LevelTimer = 0f;
        State = LevelState.RUNNING;
    }

    /// <summary>
    /// Pauses the game, disabling the flow of time and showing the pause menu.
    /// </summary>
    public void PauseGame() {
        if (State != LevelState.RUNNING) {
            return;
        }

        State = LevelState.PAUSED;
        Time.timeScale = 0f;

        Instantiate(pauseCanvasPrefab);
    }

    /// <summary>
    /// Unpauses the game, restoring the flow of time and destroying the pause menu.
    /// </summary>
    public void UnpauseGame() {
        if (State != LevelState.PAUSED) {
            return;
        }

        State = LevelState.RUNNING;
        Time.timeScale = 1f;

        var canvas = FindObjectOfType<PauseMenuCanvas>();
        if (canvas) {
            Destroy(canvas.gameObject);
        }
    }

    /// <summary>
    /// Causes a victory for the player, saving their best level time and showing the victory screen.
    /// </summary>
    public void Victory() {
        State = LevelState.VICTORY;

        SaveState.UpdateLevelRecord(currentLevel, LevelTimer, CurrentStarCount);

        Instantiate(levelCompleteCanvasPrefab);
    }

    /// <summary>
    /// Causes a game over for the player and shows the game over screen.
    /// </summary>
    public void GameOver() {
        State = LevelState.GAMEOVER;

        Instantiate(gameOverCanvasPrefab);
    }

    /// <summary>
    /// Starts an asynchronous process to proceed to the next level.
    /// </summary>
    public void NextLevel() {
        StartCoroutine(NextLevelCoroutine());
    }

    private IEnumerator NextLevelCoroutine() {
        // Find the next level to load (or load the level selection if the game has been completed)
        AsyncOperation async;
        if (currentLevel + 1 > maxLevels) {
            async = LoadLevelSelection();
        } else {
            async = LoadLevel(currentLevel + 1);
        }

        // If the victory screen is currently showing, wait for its fadeout animation
        var canvas = FindObjectOfType<LevelCompleteCanvas>();
        if (canvas) {
            yield return canvas.StartCoroutine(canvas.NextLevelAnimation());
        }

        // Allow the loaded scene to activate and start the timer
        async.allowSceneActivation = true;
        StartLevel();
    }

    /// <summary>
    /// Starts an asynchronous process to restart the current level.
    /// </summary>
    public void RestartLevel() {
        StartCoroutine(RestartLevelCoroutine());
    }

    private IEnumerator RestartLevelCoroutine() {
        // Reload the current level asynchronously
        var async = LoadLevel(currentLevel);

        // If the game over screen is currently showing, wait for its fadeout animation
        var canvas = FindObjectOfType<GameOverCanvas>();
        if (canvas) {
            yield return canvas.StartCoroutine(canvas.RestartLevelAnimation());
        }

        // Allow the loaded scene to activate and start the timer
        async.allowSceneActivation = true;
        StartLevel();
    }

}
