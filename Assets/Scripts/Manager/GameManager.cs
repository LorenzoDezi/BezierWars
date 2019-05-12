using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

public class ScoreChangeEvent : UnityEvent<int> {

}

public class GameStateChangeEvent : UnityEvent<GameState>
{

}

public enum GameState
{
    Menu, Survival, TimeLimit, Pause, GameOver, Training
}


public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [Header("Input axis")]
    private string exitAxis = "Exit";

    [Header("Gameplay parameters")]
    //The score threshold above which the difficulty will raise
    [SerializeField]
    private int scoreThreshold = 500;
    //The spawn time interval decrease at score reaching the threshold
    [SerializeField]
    private float spawnIntervalDecrease = 5f;

    [Header("Prefabs to spawn")]
    [SerializeField]
    private GameObject playerPrefab;
    private GameObject currentPlayer;
    [SerializeField]
    private GameObject bezierSpawnerPrefab;
    private GameObject currentBezierSpawner;

    private int currentScore;
    private int currentScoreThreshold;
    private ScoreChangeEvent scoreChangeEvent;
    //TODO: Refactor of this shit
    private bool hasWon;

    // First 10 best scores of the current game
    //TODO - some sort of memorization on the browser (using cache maybe)
    [SerializeField]
    private List<int> survivalLeaderBoard;
    [SerializeField]
    private List<int> timeLimitLeaderBoard;

    private GameState state;
    //Useful when game goes to pause mode
    private Stack<GameState> previousStates;
    private List<EnemySpawner> spawners;
    private float previousTimeScale;

    public GameStateChangeEvent onGameStateChange;
    private TimeLimitManager timeLimitManager;

    public static Dictionary<GameState, List<int>> LeaderBoards { get; private set; }

    public static GameState CurrentState()
    {
        return instance.state;
    }

    public static TimeLimitManager GetTimeLimitManager()
    {
        return instance.timeLimitManager;
    }

    public static GameStateChangeEvent OnGameStateChange()
    {
        return instance.onGameStateChange;
    }

    public static void IncreaseScore(int score)
    {
        instance.currentScore += score;
        instance.scoreChangeEvent.Invoke(instance.currentScore);
        if (instance.currentScore >= instance.currentScoreThreshold)
        {
            instance.currentScoreThreshold = instance.currentScore + instance.scoreThreshold * 2;
            instance.spawners.ForEach(
                (obj) => {
                    obj.SpawnInterval = obj.SpawnInterval - instance.spawnIntervalDecrease;
                    obj.MaxEnemiesCanSpawn = obj.MaxEnemiesCanSpawn + 2;
                });
        }
    }

    public static void GameOver()
    {
        if (instance.state == GameState.Survival)
            UpdateLeaderboard();
        instance.state = GameState.GameOver;
        OnGameStateChange().Invoke(instance.state);
    }

    private static void UpdateLeaderboard()
    {
        var leaderBoard = LeaderBoards[instance.state];
        var index = leaderBoard.FindIndex((x) => x < instance.currentScore);
        if (index != -1 && !leaderBoard.Contains(instance.currentScore))
        {
            leaderBoard.Add(instance.currentScore);
            LeaderBoards[instance.state] = leaderBoard.OrderByDescending(val => val).ToList();
            LeaderBoards[instance.state].RemoveAt(leaderBoard.Count - 1);
        }
    }

    public static int GetCurrentScore()
    {
        return instance.currentScore;
    }

    public static GameObject GetCurrentPlayer()
    {
        return instance.currentPlayer;
    }

    public static GameObject GetCurrentBezierSpawner()
    {
        return instance?.currentBezierSpawner;
    }

    public static ScoreChangeEvent OnScoreChanged()
    {
        return instance.scoreChangeEvent;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            scoreChangeEvent = new ScoreChangeEvent();
            onGameStateChange = new GameStateChangeEvent();
            LeaderBoards = new Dictionary<GameState, List<int>>();
            LeaderBoards.Add(GameState.TimeLimit, timeLimitLeaderBoard);
            LeaderBoards.Add(GameState.Survival, survivalLeaderBoard);
            previousStates = new Stack<GameState>();
            previousTimeScale = 1f;
        }
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        instance.currentScoreThreshold = scoreThreshold;
        var spawns = UnityEngine.GameObject.FindGameObjectsWithTag("Spawn");
        if (spawns.Count() != 0)
        {
            spawners = spawns.ToList().ConvertAll((obj) => obj.GetComponent<EnemySpawner>());
        }
        EnterMenu();
    }

    private void Update()
    {
        if (Input.GetButtonDown(exitAxis) && !hasWon 
            && GameState.Menu != state && GameState.GameOver != state && GameState.Pause != state)
            EnterPause();
    }

    private void SetPlayerInput(bool active)
    {
        currentPlayer.GetComponent<PlayerInputComponent>().enabled = active;
        currentBezierSpawner.GetComponent<BezierSpawner>().enabled = active;
    }

    #region StateMethods
    private void GameReset()
    {
        GameObject.FindGameObjectsWithTag("Player").ToList().ForEach((obj) => GameObject.Destroy(obj));
        GameObject.FindGameObjectsWithTag("Enemy").ToList().ForEach((obj) => GameObject.Destroy(obj));
        currentPlayer = GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        currentBezierSpawner = GameObject.Instantiate(bezierSpawnerPrefab, Vector3.zero, Quaternion.identity);
        var bezSpawnComp = currentBezierSpawner.GetComponent<BezierSpawner>();
        //TODO: Maybe refactor player -> you see the problem with the bezierSpawner? 
        currentPlayer.GetComponent<PlayerInputComponent>().SetBezierSpawner(bezSpawnComp);
        currentPlayer.GetComponent<PlayerController>().SetBezierSpawner(bezSpawnComp);
        currentBezierSpawner.GetComponent<FollowTargetComponent>().SetTargetToFollow(currentPlayer.transform);
        spawners.ForEach((spwn) => {
            spwn.SetTarget(currentPlayer.transform);
            spwn.Reset();
        });
        Camera.main.transform.position = new Vector3(0, 0, -10);
        //TODO: Refactor with a property
        currentScore = 0;
        currentScoreThreshold = scoreThreshold;
        hasWon = false;
        scoreChangeEvent.Invoke(currentScore);
        Camera.main.GetComponent<CameraController>().SetTransformToFollow(currentPlayer.transform);
        Camera.main.GetComponent<Animation>().Play("MenuAnimation");
    }

    public static void EnterPlacingHermite()
    {
        instance.previousStates.Push(instance.state);
        Time.timeScale = 0.4f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        instance.previousTimeScale = Time.timeScale;
    }

    public static void ExitPlacingHermite()
    {
        instance.state = instance.previousStates.Pop();
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        instance.previousTimeScale = Time.timeScale;
    }

    public void EnterMenu()
    {
        GameReset();
        this.state = GameState.Menu;
        spawners.ForEach((spwn) => spwn.enabled = false);
        Destroy(instance.timeLimitManager);
        instance.timeLimitManager = null;
        SetPlayerInput(false);
        onGameStateChange.Invoke(state);
    }

    public void EnterSurvival()
    {
        this.state = GameState.Survival;
        spawners.ForEach((spwn) => spwn.enabled = true);
        Camera.main.GetComponent<Animation>().Play("PlayAnimation");
        SetPlayerInput(true);
        onGameStateChange.Invoke(state);
    }

    public void EnterTimeLimit()
    {
        this.state = GameState.TimeLimit;
        spawners.ForEach((spwn) => spwn.enabled = true);
        Camera.main.GetComponent<Animation>().Play("PlayAnimation");
        SetPlayerInput(true);
        instance.timeLimitManager = gameObject.AddComponent<TimeLimitManager>();
        instance.timeLimitManager.EndGameEvent.AddListener(OnEndGame);
        currentBezierSpawner.GetComponent<BezierSpawner>().OnBezierCreated.AddListener(
            () => instance.timeLimitManager?.BezierConsumed());
        onGameStateChange.Invoke(state);
    }

    public void OnEndGame()
    {
        SetPlayerInput(false);
        hasWon = true;
        spawners.ForEach((spwn) => spwn.enabled = false);
        UpdateLeaderboard();
        Camera.main.GetComponent<Animation>().Play("MenuAnimation");
    }

    public void EnterPause()
    {
        Time.timeScale = 0;
        SetPlayerInput(false);
        this.previousStates.Push(state);
        this.state = GameState.Pause;
        onGameStateChange.Invoke(state);
    }

    public void ExitPause()
    {
        SetPlayerInput(true);
        this.state = this.previousStates.Pop();
        Time.timeScale = instance.previousTimeScale;
        onGameStateChange.Invoke(state);
    }

    public void EnterTraining()
    {
        this.state = GameState.Training;
        Camera.main.GetComponent<Animation>().Play("PlayAnimation");
        SetPlayerInput(true);
        onGameStateChange.Invoke(state);
    }

    public void BackToMenuFromPause()
    {
        Time.timeScale = 1;
        EnterMenu();
    }
    #endregion




}
