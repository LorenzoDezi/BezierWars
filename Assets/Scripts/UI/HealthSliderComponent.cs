using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSliderComponent : MonoBehaviour
{
    [SerializeField]
    HealthComponent playerHealth;

    void Start()
    {
        playerHealth.HealthChange.AddListener(UpdateSlider);
        GetComponent<Slider>().maxValue = playerHealth.MaxHealth;
    }

    void UpdateSlider(float value)
    {
        GetComponent<Slider>().value = value;
    }
}
