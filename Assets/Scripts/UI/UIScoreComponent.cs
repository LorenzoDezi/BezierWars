using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreComponent : MonoBehaviour
{
    [SerializeField]
    private List<Text> texts;
    [SerializeField]
    private GameState state;

    void Start()
    {
        GameManager.OnGameStateChange().AddListener(UpdateLeaderboard);
    }

    void UpdateLeaderboard(GameState state)
    {
        if (GameState.Menu != state) return;
        List<int> leaderboard = GameManager.LeaderBoards[this.state];
        if (texts.Count < leaderboard.Count) return;
        for(int i = 0; i < leaderboard.Count; i++)
            texts[i].text = (i+1) + " - " + leaderboard[i];
    }
}
