using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(HealthComponent))]
public class AttackBezierCurveComponent : BezierCurveComponent
{

    HealthComponent healthComp;

    protected override void Start()
    {
        base.Start();
        GetComponent<Collider2D>().isTrigger = true;
    }

    public void PowerUp(BulletBehaviorComponent bullet)
    {
        if (bullet.CompareTag("Player"))
            bullet.Damage = bullet.Damage * 4f;
    }
}
