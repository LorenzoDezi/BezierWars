using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(LineRenderer))]
public class AttackBezierComponent : MonoBehaviour, IDamageable
{

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    public void PowerUp(BulletBehaviorComponent bullet)
    {
        if (bullet.CompareTag("Player"))
            bullet.Damage = bullet.Damage * 4f;
    }

    public void Damaged()
    {
        //The attack bezier component can't be damaged!
    }

    public void Die()
    {
        GameObject.Destroy(gameObject);
        //TODO: Signal to the BezierSpawner!
    }
}
