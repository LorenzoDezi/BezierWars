using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class DefenseBezierComponent : BezierCurveComponent, IDamager
{
    [SerializeField]
    private float damage;

    public float Damage { get => damage; set => damage = value; }

    protected override void Start()
    {
        base.Start();
        GetComponent<Collider2D>().isTrigger = false;
    }

}
