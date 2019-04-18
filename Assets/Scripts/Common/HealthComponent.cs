using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(IDamageable))]
public class HealthComponent : MonoBehaviour
{
    [SerializeField]
    private float maxValue = 100f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamager damager = collision.collider.GetComponent<IDamager>();
        HandleCollision(damager);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(transform.tag))
            return;
        IDamager damager = collision.GetComponent<IDamager>();
        HandleCollision(damager);
    }

    public void HandleCollision(IDamager damager)
    {
        if (damager == null)
            return;
        maxValue -= damager.Damage;
        if (maxValue > 0)
            GetComponent<IDamageable>().Damaged();
        else
        {
            GetComponent<IDamageable>().Die();
            Destroy(this);
        }
    }

}
