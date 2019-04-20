﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SuicideShipController : MonoBehaviour, IEnemyController, IDamageable, IDamager
{
    private Transform target;
    [SerializeField]
    private float engineIntensity;
    [SerializeField]
    private float damage;

    [Header("Score parameters")]
    [SerializeField]
    private int scoreValue = 100;
    private UnityEvent onDefeat;

    public float Damage { get => damage; set => damage = value; }

    public UnityEvent OnDefeat => onDefeat;

    private void Awake()
    {
        onDefeat = new UnityEvent();
    }

    public void Damaged()
    {
        //TODO: particles, ecc
    }

    public void Die()
    {
        //TODO: particles, ecc
        OnDefeat.Invoke();
        Destroy(gameObject, 0.2f);
    }

    public void Move(Vector2 direction, float speedFactor)
    {
        GetComponent<Rigidbody2D>().AddForce(direction * speedFactor * engineIntensity,
            ForceMode2D.Force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(target != null && collision.collider.gameObject == target.gameObject)
            Die();
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void FixedUpdate()
    {
        LookTarget();
    }

    private void LookTarget()
    {
        if (target == null) return;
        var dir = target.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }
}
