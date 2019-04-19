using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class DefenseBezierComponent : MonoBehaviour, IDamageable, IDamager
{

    private Transform transformToFollow;
    private Vector3 offset;
    [SerializeField]
    private float damage;

    public float Damage { get => damage; set => damage = value; }

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = false;
    }

    private void Update()
    {
        if (transformToFollow == null)
            return;
        transform.position = transformToFollow.position + offset;
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

    public void SetTargetToFollow(Transform transformToFollow)
    {
        this.transformToFollow = transformToFollow;
        this.offset = transform.position - transformToFollow.position;
    }
}
