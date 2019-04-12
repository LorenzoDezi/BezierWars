using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Renderer))]
public class FlankShipController : MonoBehaviour, IEnemyController, Damageable
{
    [Header("Movement parameters")]
    [SerializeField]
    private float engineIntensity = 5f;
    [SerializeField]
    private float flankIntensity = 2f;
    [SerializeField]
    private float flankAwareness = 5f;
    [SerializeField]
    private float flankRate = 3f;
    private float lastFlank = float.NegativeInfinity;
    private Transform target;

    public void Move(Vector2 direction, float speedFactor)
    {
        GetComponent<Rigidbody2D>().AddForce(direction * speedFactor * engineIntensity,
            ForceMode2D.Force);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }


    private void FixedUpdate()
    {
        if(!Camera.main.IsObjectVisible(GetComponent<Renderer>()))
            return;
        Attack();
        Flank();
    }

    private void Attack()
    {
        var dir = target.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        GetComponentInChildren<BulletSpawnerComponent>().Spawn();
    }

    private void Flank()
    {
        if (Time.time <= lastFlank + flankRate) return;
        lastFlank = Time.time;
        var hit = Physics2D.Raycast((Vector2)transform.position, (Vector2)transform.up,
            flankAwareness,
            LayerMask.GetMask("Ships"));
        //Generate -1 or +1 (randomize direction of the flank)
        int direction = Random.Range(0, 2) * 2 - 1;
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            GetComponent<Rigidbody2D>().AddForce(
                transform.right * direction * flankIntensity, ForceMode2D.Impulse);
        }
    }

    public void Damaged()
    {
        Debug.Log("Damaged!");
        //TODO: Particle system and shit
    }

    public void Die()
    {
        Debug.Log(string.Format("{0} eliminated!", gameObject.name));
        Destroy(gameObject, 1f);
        //TODO: Particle system and shit
    }
}
