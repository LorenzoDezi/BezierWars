using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeChangeEvent : UnityEvent<float>
{

}

public class TimeLimitManager : MonoBehaviour
{
    [SerializeField]
    private float timeFactor = 1f;
    [SerializeField]
    private float maxTime = 100f;
    public float MaxTime { get => maxTime; }
    [SerializeField]
    private float bezierConsumePrice = 20f;
    private float currentTime;
    public float CurrTime { get => currentTime; }
    public TimeChangeEvent TimeChangeEvent;
    public UnityEvent EndGameEvent;

    // Update is called once per frame
    void Update()
    {
        if (currentTime <= 0) return;
        currentTime -= timeFactor * Time.deltaTime;
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

    public void BezierConsumed()
    {
        if (currentTime <= 0) return;
        currentTime -= bezierConsumePrice;
        TimeChangeEvent.Invoke(currentTime);
        if (currentTime <= 0)
            EndGameEvent.Invoke();
    }

}