using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float Damage { get => damage; set => damage = value; }

    public void Damaged()
    {
        //TODO: particles, ecc
    }

    public void Die()
    {
        //TODO: particles, ecc
        Destroy(gameObject, 0.2f);
    }

    public void Move(Vector2 direction, float speedFactor)
    {
        GetComponent<Rigidbody2D>().AddForce(direction * speedFactor * engineIntensity,
            ForceMode2D.Force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject == target.gameObject)
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
        var dir = target.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }
}
