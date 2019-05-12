using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class TimeLimitSliderComponent : MonoBehaviour
{
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        TimeLimitManager timeManager = GameManager.GetTimeLimitManager();
        slider.maxValue = timeManager.MaxTime;
        slider.value = timeManager.CurrTime;
        timeManager.TimeChangeEvent.AddListener(OnTimeChange);
    }

    void OnTimeChange(float value)
    {
        slider.value = value;
    }

}
