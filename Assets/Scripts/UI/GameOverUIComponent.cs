using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIComponent : MonoBehaviour
{
    [SerializeField]
    private Text gameOverText;
    [SerializeField]
    private Text scoreText;
    private bool isSurvival = false;

    void Start()
    {
        GameManager.OnGameStateChange().AddListener(OnGameStateChange);
    }

    void OnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Survival:
                isSurvival = true;
                break;
            case GameState.TimeLimit:
                isSurvival = false;
                TimeLimitManager timeManager = GameManager.GetTimeLimitManager();
                timeManager.EndGameEvent.AddListener(OnEndGame);
                break;
            case GameState.GameOver:
                OnGameOver();
                break;
        }
        
    }

    void OnEndGame()
    {
        gameOverText.text = "YOU WIN";
        scoreText.text = "SCORE " + GameManager.GetCurrentScore();
    }

    void OnGameOver()
    {
        gameOverText.text = "GAME OVER";
        if (isSurvival)
            scoreText.text = "SCORE " + GameManager.GetCurrentScore();
        else
            scoreText.text = "";
    }


}
