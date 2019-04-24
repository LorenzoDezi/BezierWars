﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HealthSliderComponent))]
public class BezierHealthSliderComponent : MonoBehaviour
{
    [SerializeField]
    private BezierSpawner spawner;
    [SerializeField]
    private BezierType type;
    // Start is called before the first frame update
    public void SetSpawner(BezierSpawner spawner)
    {
        spawner.OnBezierCreated.AddListener(OnBezierCreated);
    }

    void OnBezierCreated(GameObject bezier, BezierType type)
    {
        if (this.type != type) return;
        GetComponent<HealthSliderComponent>().SetHealthComponent(bezier.GetComponent<HealthComponent>());
        bezier.GetComponent<BezierBuilderComponent>().Disabled.AddListener(OnBezierDisabled);
    }

    void OnBezierDisabled()
    {
        var slider = GetComponent<Slider>();
        slider.value = slider.maxValue;
    }
}

