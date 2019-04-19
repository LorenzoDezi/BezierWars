using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierHealthSliderComponent : MonoBehaviour
{
    [SerializeField]
    private BezierSpawner spawner;
    [SerializeField]
    private BezierType type;
    // Start is called before the first frame update
    void Start()
    {
        spawner.OnBezierCreated.AddListener(OnBezierCreated);
    }

    void OnBezierCreated(HealthComponent healthComp, BezierType type)
    {
        if (this.type != type) return;
        GetComponent<HealthSliderComponent>().SetHealthComponent(healthComp);
    }
}

