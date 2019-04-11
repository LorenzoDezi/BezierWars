using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviorComponent : MonoBehaviour
{
    [SerializeField]
    private float health = 5f;
    [SerializeField]
    private float damage = 10f;

    // Update is called once per frame
    void Update()
    {
        health -= Time.deltaTime;
        if (health <= 0)
            GameObject.Destroy(gameObject);
        //TODO Add particle system and things like that
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO Handle damage 
        //TODO Add particle systems
        if(!collision.collider.CompareTag("Bullet"))
            GameObject.Destroy(gameObject);
    }


}
