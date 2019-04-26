using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class DefenseBezierComponent : MonoBehaviour, IDamageable, IDamager
{
    [SerializeField]
    private float damage;

    public float Damage { get => damage; set => damage = value; }

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = false;
    }

    public void Damaged()
    {
        //TODO Add particle systems and effects
    }

    public void Die()
    {
        GetComponent<BezierBuilderComponent>().Disabled.Invoke();
        Destroy(gameObject, 0.2f);
    }

}
