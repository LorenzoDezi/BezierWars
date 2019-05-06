using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreComponent : MonoBehaviour
{
    [SerializeField]
    private List<Text> texts;

    void Start()
    {
        GameManager.OnGameStateChange().AddListener(UpdateLeaderboard);
    }

    void UpdateLeaderboard(GameState state)
    {
        if (GameState.Menu != state) return;
        int[] leaderboard = GameManager.GetLeaderboard();
        if (texts.Count < leaderboard.Length) return;
        for(int i = 0; i < leaderboard.Length; i++)
        {
            texts[i].text = (i+1) + " - " + leaderboard[i];
        }
    }
}
