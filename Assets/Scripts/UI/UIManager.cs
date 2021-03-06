﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    [Header("GameState dependent UI elements")]
    [SerializeField]
    private List<GameObject> HUDUIElements;
    [SerializeField]
    private List<GameObject> timeLimitUIElements;
    [SerializeField]
    private List<GameObject> pauseUIElements;
    [SerializeField]
    private List<GameObject> menuUIElements;
    [SerializeField]
    private List<GameObject> gameOverUIElements;
    [SerializeField]
    private List<GameObject> trainingUIElements;
    [SerializeField]
    private HealthSliderComponent playerHealthSlider;
    [SerializeField]
    private Text gameOverScoreText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        GameManager.OnGameStateChange().AddListener(OnGameStateChange);
    }

    void OnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                HUDUIElements.ForEach((obj) => obj.SetActive(false));
                pauseUIElements.ForEach((obj) => obj.SetActive(false));
                gameOverUIElements.ForEach((obj) => obj.SetActive(false));
                trainingUIElements.ForEach((obj) => obj.SetActive(false));
                timeLimitUIElements.ForEach((obj) => obj.SetActive(false));
                menuUIElements.ForEach((obj) => obj.SetActive(true));
                playerHealthSlider.SetHealthComponent(GameManager.GetCurrentPlayer()?
                    .GetComponent<HealthComponent>());
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
                timeLimitUIElements.ForEach((obj) => obj.SetActive(true));
                GameManager.GetTimeLimitManager().EndGameEvent.AddListener(OnGameOver);
                break;
            case GameState.Pause:
                menuUIElements.ForEach((obj) => obj.SetActive(false));
                gameOverUIElements.ForEach((obj) => obj.SetActive(false));
                pauseUIElements.ForEach((obj) => obj.SetActive(true));
                break;
            case GameState.GameOver:
                OnGameOver();
                break;
            case GameState.Training:
                menuUIElements.ForEach((obj) => obj.SetActive(false));
                pauseUIElements.ForEach((obj) => obj.SetActive(false));
                gameOverUIElements.ForEach((obj) => obj.SetActive(false));
                HUDUIElements.ForEach((obj) => obj.SetActive(true));
                trainingUIElements.ForEach((obj) => obj.SetActive(true));
                break;
        }
    }

    private void OnGameOver()
    {
        menuUIElements.ForEach((obj) => obj.SetActive(false));
        HUDUIElements.ForEach((obj) => obj.SetActive(false));
        pauseUIElements.ForEach((obj) => obj.SetActive(false));
        gameOverUIElements.ForEach((obj) => obj.SetActive(true));
        timeLimitUIElements.ForEach((obj) => obj.SetActive(false));
    }
}