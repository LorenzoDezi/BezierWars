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
    protected override void Start()
    {
        base.Start();
        GetComponent<Collider2D>().isTrigger = true;
    }

    public void PowerUp(BulletBehaviorComponent bullet)
    {
        if (!bullet.CompareTag("Player") || bullet.Powered) return;
        bullet.Die();
        GetComponent<BulletSpawnerComponent>().Spawn(
            bullet.transform.position + bullet.transform.up * 2f, 
            bullet.transform.rotation,
            true
            );
    }

    public override void Die()
    {
        GetComponent<BezierBuilderComponent>().Disabled.Invoke(BezierType.Attack);
        base.Die();
    }
}
