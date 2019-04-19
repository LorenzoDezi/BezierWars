using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviorComponent : MonoBehaviour, IDamager, IDamageable
{
    [SerializeField]
    private float damage = 10f;

    public float Damage {
        get => damage;
        set => damage = value;
    }

    void FixedUpdate()
    {
        float velocity = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 
            velocity * Time.fixedDeltaTime);
        if(hit.collider != null)
        {
            if(hit.collider.isTrigger)
            {
                var attackBezComp = hit.collider.GetComponent<AttackBezierComponent>();
                if (attackBezComp != null)
                    attackBezComp.PowerUp(this);
            } else
            {
                GameObject.Destroy(gameObject);
                var healthComp = hit.collider.GetComponent<HealthComponent>();
                if (healthComp != null && !hit.collider.CompareTag(transform.tag))
                    healthComp.HandleCollision(this);
            }
            
        }
    }

    public void Damaged()
    {
        //The bullet can't be damaged
    }

    public void Die()
    {
        //Particle System
        GameObject.Destroy(gameObject);
    }
}
