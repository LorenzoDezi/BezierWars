using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DestructibleComponent))]
public class SuicideShipController : MonoBehaviour, IEnemyController, IDamageable, IDamager
{
    private Transform target;
    [SerializeField]
    private float engineIntensity;
    [SerializeField]
    private float damage;

    public float Damage { get => damage; set => damage = value; }

    [Header("Score parameters")]
    [SerializeField]
    private int scoreValue = 100;
    private UnityEvent onDefeat;
    public UnityEvent OnDefeat => onDefeat;

    [Header("Sounds effects")]
    [SerializeField]
    private AudioClip deathSound;
    [SerializeField]
    private AudioClip tickSound;
    [SerializeField]
    private float tickDistanceFactor;
    [SerializeField]
    private float tickSoundVolumeBase = 6f;
    private float lastTicked = float.NegativeInfinity;

    private void Awake()
    {
        onDefeat = new UnityEvent();
    }

    public void Damaged()
    {
        //The suicide ship do nothing when damaged
    }

    public void Die()
    {
        OnDefeat.Invoke();
        GameManager.IncreaseScore(scoreValue);
        SoundManager.PlaySound(deathSound);
        GetComponent<DestructibleComponent>().StartDestroy();
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

    private void Update()
    {
        if (target == null) return;
        var tickInterval = tickDistanceFactor * Vector3.Distance(
            target.position, transform.position);
        if(Time.time > lastTicked + tickInterval)
        {
            lastTicked = Time.time;
            SoundManager.PlaySound(tickSound, tickSoundVolumeBase - tickInterval);
        }
    }

    private void LookTarget()
    {
        if (target == null) return;
        var dir = target.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }
}
