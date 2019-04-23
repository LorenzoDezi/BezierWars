using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScoreChangeEvent : UnityEvent<int> {

}

public class GameStateChangeEvent : UnityEvent<GameState>
{

}

public enum GameState
{
    Menu, Arena,
    Training, Pause
}


public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private int currentScore;
    // First 10 best scores of the current game
    //TODO - some sort of memorization on the browser (using cache maybe)
    private List<int> leaderBoard;
    private List<EnemySpawner> spawners;
    //The score threshold above which the difficulty will raise
    [Header("Gameplay parameters")]
    [SerializeField]
    private PlayerInputComponent player;
    [SerializeField]
    private BezierSpawner bezSpawner;
    [SerializeField]
    private int scoreThreshold = 500;
    private int currentScoreThreshold;
    //The spawn time interval decrease at score reaching the threshold
    [SerializeField]
    private float spawnIntervalDecrease = 5f;

    //TODO: Refactor on UI
    [Header("UIElements")]
    [SerializeField]
    private List<GameObject> arenaUIElements;
    [SerializeField]
    private List<GameObject> trainingUIElements;
    [SerializeField]
    private List<GameObject> menuUIElements;
    [SerializeField]
    private List<GameObject> pauseUIElements;

    [Header("Input axis")]
    [SerializeField]
    private string pauseAxis;

    private GameState state;
    public GameStateChangeEvent onGameStateChange;
    private ScoreChangeEvent scoreChangeEvent;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            leaderBoard = new List<int>();
            scoreChangeEvent = new ScoreChangeEvent();
            onGameStateChange = new GameStateChangeEvent();
        }
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetButtonDown(pauseAxis))
        {
            Pause();
        }
    }

    private void Start()
    {
        instance.currentScoreThreshold = scoreThreshold;
        var spawns = UnityEngine.GameObject.FindGameObjectsWithTag("Spawn");
        if(spawns.Count() != 0)
        {
            spawners = spawns.ToList().ConvertAll((obj) => obj.GetComponent<EnemySpawner>());
        }
        SetMenuState();
    }

    public void SetMenuState()
    {
        //Reset spawners
        spawners.ForEach((spawner) =>
        {
            spawner.gameObject.SetActive(false);
        });
        //Reset player
        ResetPlayer();
        //Reset spawner
        bezSpawner.enabled = false;
        bezSpawner.transform.position = Vector3.zero;
        //Reset camera
        Camera.main.transform.position = Vector3.zero;
        //Reset score
        currentScore = 0;
        currentScoreThreshold = scoreThreshold;
        //Reset GUI
        arenaUIElements.ToList().ForEach((obj) => obj.gameObject.SetActive(false));
        trainingUIElements.ToList().ForEach((obj) => obj.gameObject.SetActive(false));
        menuUIElements.ForEach((obj) => obj.gameObject.SetActive(true));
        var prevState = state;
        state = GameState.Menu;
        if(prevState != state)
            onGameStateChange.Invoke(instance.state);
    }

    public void SetArenaState()
    {
        spawners.ForEach((spawner) => spawner.gameObject.SetActive(true));
        player.enabled = true;
        player.GetComponent<Rigidbody2D>().simulated = true;
        bezSpawner.enabled = true;
        menuUIElements.ForEach((obj) => obj.gameObject.SetActive(false));
        trainingUIElements.ToList().ForEach((obj) => obj.gameObject.SetActive(false));
        arenaUIElements.ToList().ForEach((obj) => obj.gameObject.SetActive(true));
        state = GameState.Arena;
        onGameStateChange.Invoke(instance.state);
    }

    public void SetTrainingState()
    {
        player.enabled = true;
        bezSpawner.enabled = true;
        menuUIElements.ForEach((obj) => obj.gameObject.SetActive(false));
        arenaUIElements.ToList().ForEach((obj) => obj.gameObject.SetActive(false));
        trainingUIElements.ToList().ForEach((obj) => obj.gameObject.SetActive(true));
        state = GameState.Training;
        onGameStateChange.Invoke(instance.state);
    }

    private void ResetPlayer()
    {
        player.enabled = false;
        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
        player.GetComponent<Rigidbody2D>().simulated = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<HealthComponent>().Refill();
    }

    public void Pause()
    {
        if (GameState.Menu == state)
            return;
        if (Time.timeScale == 0)
            Resume();
        else
        {
            if(player != null) player.enabled = false;
            pauseUIElements.ToList().ForEach((obj) => obj.gameObject.SetActive(true));
            Time.timeScale = 0;
        }          
    }

    public void Resume()
    {
        player.enabled = true;
        pauseUIElements.ToList().ForEach((obj) => obj.gameObject.SetActive(false));
        Time.timeScale = 1;
    }

    public void Exit()
    {
        Resume();
        SetMenuState();
    }

    public static GameState CurrentState()
    {
        return instance.state;
    }

    public static void IncreaseScore(int score)
    {
        instance.currentScore += score;
        instance.scoreChangeEvent.Invoke(instance.currentScore);
        if(instance.currentScore >= instance.currentScoreThreshold)
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
        if(instance.leaderBoard.Count < 10)
        {
            instance.leaderBoard.Add(instance.currentScore);
            instance.leaderBoard.Sort((x, y) => x >= y ? x : y);
        } else
        {
            var index = instance.leaderBoard.FindIndex((x) => x < instance.currentScore);
            if(index != -1)
            {
                instance.leaderBoard.RemoveAt(index);
                instance.leaderBoard.Add(instance.currentScore);
                instance.leaderBoard.Sort((x, y) => x >= y ? x : y);
            }
        }
        instance.currentScore = 0;
    }

    public static int GetCurrentScore()
    {
        return instance.currentScore;
    }

    public static int GetPositionOnLeaderBoard()
    {
        return instance.leaderBoard.FindIndex((x) => x < instance.currentScore);
    }

    public static ScoreChangeEvent OnScoreChanged()
    {
        return instance.scoreChangeEvent;
    }

    public static GameStateChangeEvent OnGameStateChange()
    {
        return instance.onGameStateChange;
    }


}
