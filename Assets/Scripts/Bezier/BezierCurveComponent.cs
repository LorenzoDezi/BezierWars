using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(HealthComponent))]
public class BezierCurveComponent : MonoBehaviour, IDamageable
{

    HealthComponent healthComp;

    protected virtual void Start()
    {
        healthComp = GetComponent<HealthComponent>();
    }

    public void Damaged()
    {
        Color color = GetComponent<LineRenderer>().material.color;
        color.a = healthComp.CurrentHealth / healthComp.MaxHealth;
        GetComponent<LineRenderer>().material.color = color;
    }

    public void Die()
    {
        foreach (BezierNodeComponent node in GetComponentsInChildren<BezierNodeComponent>())
        {
            node.transform.parent = null;
            node.DestroyNode();
        }
        GetComponent<BezierBuilderComponent>().Disabled.Invoke();
        Destroy(gameObject);
    }
}
