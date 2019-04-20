using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class HealthChangeEvent : UnityEvent<float>
{

}

[RequireComponent(typeof(IDamageable))]
public class HealthComponent : MonoBehaviour
{
    [SerializeField]
    protected float maxValue = 100f;
    protected float currentValue;
    public UnityEvent<float> HealthChange;
    public float MaxHealth { get => maxValue; }

    private void Awake()
    {
        HealthChange = new HealthChangeEvent();
    }

    private void Start()
    {
        this.currentValue = maxValue;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(transform.tag))
            return;
        IDamager damager = collision.collider.GetComponent<IDamager>();
        HandleCollision(damager);
    }

    public void HandleCollision(IDamager damager)
    {
        if (damager == null)
            return;
        currentValue -= damager.Damage;
        HealthChange.Invoke(currentValue);
        if (currentValue > 0)
            GetComponent<IDamageable>().Damaged();
        else
        {
            GetComponent<IDamageable>().Die();
            Destroy(this);
        }
    }

    public void ForceDeath()
    {
        currentValue = 0f;
        HealthChange.Invoke(currentValue);
        GetComponent<IDamageable>().Die();
        Destroy(this);
    }

}
