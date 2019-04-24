﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    [Header("GameState dependent UI elements")]
    [SerializeField]
    private List<GameObject> HUDUIElements;
    [SerializeField]
    private List<GameObject> pauseUIElements;
    [SerializeField]
    private List<GameObject> menuUIElements;
    [SerializeField]
    private List<GameObject> gameOverUIElements;
    [SerializeField]
    private List<GameObject> trainingUIElements;

    [Header("Sliders")]
    [SerializeField]
    private List<BezierHealthSliderComponent> bezierSliders;
    [SerializeField]
    private HealthSliderComponent playerHealthSlider;

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
                trainingUIElements.ForEach((obj) => obj.SetActive(false));
                menuUIElements.ForEach((obj) => obj.SetActive(true));
                break;
            case GameState.Survival:
                pauseUIElements.ForEach((obj) => obj.SetActive(false));
                menuUIElements.ForEach((obj) => obj.SetActive(false));
                gameOverUIElements.ForEach((obj) => obj.SetActive(false));
                trainingUIElements.ForEach((obj) => obj.SetActive(false));
                HUDUIElements.ForEach((obj) => obj.SetActive(true));
                break;
            case GameState.TimeLimit:
                pauseUIElements.ForEach((obj) => obj.SetActive(false));
                menuUIElements.ForEach((obj) => obj.SetActive(false));
                gameOverUIElements.ForEach((obj) => obj.SetActive(false));
                trainingUIElements.ForEach((obj) => obj.SetActive(false));
                HUDUIElements.ForEach((obj) => obj.SetActive(true));
                break;
            case GameState.Pause:
                menuUIElements.ForEach((obj) => obj.SetActive(false));
                HUDUIElements.ForEach((obj) => obj.SetActive(false));
                gameOverUIElements.ForEach((obj) => obj.SetActive(false));
                trainingUIElements.ForEach((obj) => obj.SetActive(false));
                pauseUIElements.ForEach((obj) => obj.SetActive(true));
                break;
            case GameState.GameOver:
                menuUIElements.ForEach((obj) => obj.SetActive(false));
                HUDUIElements.ForEach((obj) => obj.SetActive(false));
                pauseUIElements.ForEach((obj) => obj.SetActive(false));
                trainingUIElements.ForEach((obj) => obj.SetActive(false));
                gameOverUIElements.ForEach((obj) => obj.SetActive(true));
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

    public static void SetSliderTargets(HealthComponent playerHealth, BezierSpawner bezSpawner)
    {
        instance.bezierSliders.ForEach((bezSlid) => bezSlid.SetSpawner(bezSpawner));
        instance.playerHealthSlider.SetHealthComponent(playerHealth);
    }
}