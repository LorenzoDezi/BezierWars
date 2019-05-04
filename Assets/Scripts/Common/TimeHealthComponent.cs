using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHealthComponent : HealthComponent
{
    [SerializeField]
    private float timeFactor = 1f;
    // Update is called once per frame
    void Update()
    {
        currentValue -= timeFactor * Time.deltaTime;
        HealthChange.Invoke(currentValue);
        if (currentValue <= 0)
            GetComponent<IDamageable>().Die();
        else
            GetComponent<IDamageable>().Damaged();
    }
}
