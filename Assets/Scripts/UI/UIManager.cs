using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> HUDUIElements;
    [SerializeField]
    private List<GameObject> pauseUIElements;
    [SerializeField]
    private List<GameObject> menuUIElements;
    [SerializeField]
    private List<GameObject> gameOverUIElements;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnGameStateChange().AddListener(onGameStateChange);
    }

    void onGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                HUDUIElements.ForEach((obj) => obj.SetActive(false));
                pauseUIElements.ForEach((obj) => obj.SetActive(false));
                gameOverUIElements.ForEach((obj) => obj.SetActive(false));
                menuUIElements.ForEach((obj) => obj.SetActive(true));
                break;
            case GameState.Survival:
                pauseUIElements.ForEach((obj) => obj.SetActive(false));
                menuUIElements.ForEach((obj) => obj.SetActive(false));
                gameOverUIElements.ForEach((obj) => obj.SetActive(false));
                HUDUIElements.ForEach((obj) => obj.SetActive(true));
                break;
            case GameState.TimeLimit:
                pauseUIElements.ForEach((obj) => obj.SetActive(false));
                menuUIElements.ForEach((obj) => obj.SetActive(false));
                gameOverUIElements.ForEach((obj) => obj.SetActive(false));
                HUDUIElements.ForEach((obj) => obj.SetActive(true));
                break;
            case GameState.Pause:
                menuUIElements.ForEach((obj) => obj.SetActive(false));
                HUDUIElements.ForEach((obj) => obj.SetActive(false));
                gameOverUIElements.ForEach((obj) => obj.SetActive(false));
                pauseUIElements.ForEach((obj) => obj.SetActive(true));
                break;
            case GameState.GameOver:
                menuUIElements.ForEach((obj) => obj.SetActive(false));
                HUDUIElements.ForEach((obj) => obj.SetActive(false));
                pauseUIElements.ForEach((obj) => obj.SetActive(false));
                gameOverUIElements.ForEach((obj) => obj.SetActive(true));
                break;
        }
    }
}