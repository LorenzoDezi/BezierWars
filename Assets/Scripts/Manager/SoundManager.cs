﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    [SerializeField]
    private AudioClip MenuMusic;
    [SerializeField]
    private AudioClip ArenaMusic;
    private bool wasPaused;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            GetComponent<AudioSource>().volume = 0.4f;
        }
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        ChangeState(GameManager.CurrentState());
        GameManager.OnGameStateChange().AddListener(ChangeState);
    }

    public static void PlaySound(AudioClip clip, float volumeScale = 4f)
    {
        instance.GetComponent<AudioSource>().PlayOneShot(clip, volumeScale);
    }

    private void ChangeState(GameState state)
    {
        var audioSrc = GetComponent<AudioSource>();
        switch (state)
        {
            case GameState.Survival:
            case GameState.TimeLimit:
                if (!wasPaused)
                {
                    audioSrc.clip = ArenaMusic;
                    audioSrc.Play();
                }
                else
                    wasPaused = false;                
                break;
            case GameState.Pause:
                wasPaused = true;
                break;
            case GameState.Menu:
                audioSrc.clip = MenuMusic;
                audioSrc.Play();
                wasPaused = false;
                break;
        }
    }
}
