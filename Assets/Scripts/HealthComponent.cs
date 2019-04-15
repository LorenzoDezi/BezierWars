using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Damageable))]
public class HealthComponent : MonoBehaviour
{
    [SerializeField]
    private float maxValue = 100f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Damager damager = collision.collider.GetComponent<Damager>();
        HandleCollision(damager);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(transform.tag))
            return;
        Damager damager = collision.GetComponent<Damager>();
        HandleCollision(damager);
    }

    private void HandleCollision(Damager damager)
    {
        if (damager == null)
            return;
        maxValue -= damager.Damage;
        if (maxValue > 0)
            GetComponent<Damageable>().Damaged();
        else
        {
            GetComponent<Damageable>().Die();
            Destroy(this);
        }
    }

}
