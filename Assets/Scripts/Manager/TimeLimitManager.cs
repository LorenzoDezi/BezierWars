using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeChangeEvent : UnityEvent<float>
{

}

public class TimeLimitManager : MonoBehaviour
{
    private float maxTime;
    public float MaxTime { get => maxTime; }
    private float currentTime;
    public float CurrTime { get => currentTime; }
    public TimeChangeEvent TimeChangeEvent;
    public UnityEvent EndGameEvent;

    // Update is called once per frame
    void Update()
    {
        if (currentTime <= 0) return;
        currentTime -= Time.deltaTime;
        TimeChangeEvent.Invoke(currentTime);
        if (currentTime <= 0)
            EndGameEvent.Invoke();
    }

    private void Awake()
    {
        currentTime = maxTime;
        TimeChangeEvent = new TimeChangeEvent();
        EndGameEvent = new UnityEvent();
    }

    public void ResetTime()
    {
        currentTime = maxTime;
        TimeChangeEvent.Invoke(currentTime);
    }

    public void Init(float maxTime)
    {
        this.maxTime = maxTime;
        currentTime = maxTime;
    }

    public void ExtraConsumed(float timeConsumed)
    {
        if (currentTime <= 0) return;
        currentTime -= timeConsumed;
        TimeChangeEvent.Invoke(currentTime);
        if (currentTime <= 0)
            EndGameEvent.Invoke();
    }
}