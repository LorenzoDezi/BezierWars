using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        GameManager.OnUpdateLeaderboard().AddListener(UpdateLeaderboard);
    }

    void UpdateLeaderboard()
    {
        int[] leaderboard = GameManager.LeaderBoards[this.state];
        if (texts.Count < leaderboard.Length) return;
        for(int i = 0; i < leaderboard.Length; i++)
            texts[i].text = (i+1) + " - " + leaderboard[i];
    }
}
