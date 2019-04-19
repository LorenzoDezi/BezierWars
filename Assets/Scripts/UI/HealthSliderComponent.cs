using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSliderComponent : MonoBehaviour
{
    [SerializeField]
    HealthComponent health;

    void Start()
    {
        if (health == null) return;
        health.HealthChange.AddListener(UpdateSlider);
        GetComponent<Slider>().maxValue = health.MaxHealth;
        GetComponent<Slider>().value = health.MaxHealth;
    }

    /// <summary>
    /// Set the health component after the start method. Used for objects
    /// not available at scene setup (maybe spawned later).
    /// </summary>
    /// <param name="health"></param>
    public void SetHealthComponent(HealthComponent health)
    {
        //Avoiding to register more than one components
        if (health != null)
        {
            health.HealthChange.RemoveListener(UpdateSlider);
        }
        health.HealthChange.AddListener(UpdateSlider);
        GetComponent<Slider>().maxValue = health.MaxHealth;
        GetComponent<Slider>().value = health.MaxHealth;
    }

    void UpdateSlider(float value)
    {
        GetComponent<Slider>().value = value;
    }
}
