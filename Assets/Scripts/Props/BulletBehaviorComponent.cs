using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviorComponent : MonoBehaviour, IDamager
{
    [SerializeField]
    private float health = 5f;
    [SerializeField]
    private float damage = 10f;

    public float Damage {
        get => damage;
        set => damage = value;
    }

    // Update is called once per frame
    void Update()
    {
        health -= Time.deltaTime;
        if (health <= 0)
            GameObject.Destroy(gameObject);
        //TODO Add particle system and things like that
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ////TODO Add particle systems
        //if (!collision.gameObject.CompareTag(transform.tag))
        //    GameObject.Destroy(gameObject);
    }

    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 
            GetComponent<Rigidbody2D>().velocity.y * Time.fixedDeltaTime);
        if(hit.collider != null)
        {
            GameObject.Destroy(gameObject);
            //TODO: All cases
            var healthComp = hit.collider.GetComponent<HealthComponent>();
            if (healthComp != null)
                healthComp.HandleCollision(this);
        }
    }


}
