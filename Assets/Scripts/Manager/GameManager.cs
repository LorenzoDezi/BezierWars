using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ScoreChangeEvent : UnityEvent<int> {

}

public class GameStateChangeEvent : UnityEvent<GameState>
{

}

public enum GameState
{
    Menu, Arena
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
    [SerializeField]
    private int scoreThreshold = 500;
    private int currentScoreThreshold;
    //The spawn time interval decrease at score reaching the threshold
    [SerializeField]
    private float spawnIntervalDecrease = 5f;
    private ScoreChangeEvent scoreChangeEvent;
    //TODO: Change when game changes from scene to scene
    [SerializeField]
    private GameState state;
    public GameStateChangeEvent onGameStateChange;

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

    private void Start()
    {
        instance.currentScoreThreshold = scoreThreshold;
        var spawns = UnityEngine.GameObject.FindGameObjectsWithTag("Spawn");
        if(spawns.Count() != 0)
        {
            spawners = spawns.ToList().ConvertAll((obj) => obj.GetComponent<EnemySpawner>());
        }
    }

    public static GameState CurrentState()
    {
        return instance.state;
    }

    public static GameStateChangeEvent OnGameStateChange()
    {
        return instance.onGameStateChange;
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

}
