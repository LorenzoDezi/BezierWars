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
        Debug.Log(name + timeFactor * Time.deltaTime * timeFactor);
        currentValue -= timeFactor * Time.deltaTime * timeFactor;
        if (currentValue <= 0)
            GetComponent<IDamageable>().Die();
    }
}
