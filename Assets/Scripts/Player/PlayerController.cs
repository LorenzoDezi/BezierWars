using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BulletSpawnerComponent))]
[RequireComponent(typeof(DestructibleComponent))]
public class PlayerController : MonoBehaviour, IDamageable
{

    [Header("Movement parameters")]
    [SerializeField]
    private float maxVelocity;
    [SerializeField]
    private float brakeIntensity = 5f;
    [SerializeField]
    private float enginePower = 5f;
    [SerializeField]
    private float boostIntensity = 2f;
    [SerializeField]
    private float torqueIntensity = 5f;
    [SerializeField]
    private float maxAngularVelocity;
    
    [Header("Attack parameters")]
    [SerializeField]
    private float attackInterval = 0.5f;
    private bool canAttack = true;
    private float timeFromLastAttack = 0f;

    [Header("Sound Effects")]
    [SerializeField]
    private AudioClip shotSound;
    [SerializeField]
    private AudioClip deathSound;
    [SerializeField]
    private AudioClip damagedSound;

    [Header("Pincer sprites")]
    [SerializeField]
    private SpriteRenderer redBluePincerSprite;
    [SerializeField]
    private SpriteRenderer greenPincerSprite;

    private bool isAccelerating = true;
    private BezierSpawner bezierSpawner;

    private void Start()
    {
        SetCommonAppeareance();
    }

    public void SetBezierSpawner(BezierSpawner spawner)
    {
        bezierSpawner = spawner;
        bezierSpawner.EnteredHermiteMode.AddListener(SetHermiteAppareance);
        bezierSpawner.ExitedHermiteMode.AddListener(SetCommonAppeareance);
    }

    private void SetHermiteAppareance()
    {
        greenPincerSprite.enabled = true;
        redBluePincerSprite.enabled = false;
    }

    private void SetCommonAppeareance()
    {
        redBluePincerSprite.enabled = true;
        greenPincerSprite.enabled = false;
    }

    void Update()
    {
        UpdateAttack();
    }

    private void UpdateAttack()
    {
        timeFromLastAttack += Time.deltaTime;
        if (timeFromLastAttack >= attackInterval)
        {
            timeFromLastAttack = 0f;
            canAttack = true;
        }
    }

    public void Rotate(float orientInput)
    {
        if (orientInput != 0 &&
                    Mathf.Abs(GetComponent<Rigidbody2D>().angularVelocity) <= maxAngularVelocity)
        {
            GetComponent<Rigidbody2D>().AddTorque(
                orientInput * torqueIntensity);
        }
    }

    public void Move(float movInput)
    {
        //We use max function because it is used to limit the player speed.
        float currentVelocity = Mathf.Max(
            Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y),
            Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x)
        );
        //Detecting changing of direction: the bool value is intended
        //to perform a "brake" if the player wants to change direction
        //after reaching a certain velocity.
        bool isDirectionChanged = (isAccelerating && movInput < 0)
                || (!isAccelerating && movInput > 0);
        if (isDirectionChanged)
        {
            isAccelerating = !isAccelerating;
            if (currentVelocity >= maxVelocity - 0.5f)
                GetComponent<Rigidbody2D>().AddForce(
                    movInput * brakeIntensity * transform.up, ForceMode2D.Impulse);
        }
        else if (movInput != 0 &&
            currentVelocity <= maxVelocity)
        {
            GetComponent<Rigidbody2D>().AddForce(
                movInput * enginePower * transform.up);
        }
    }

    public void Attack()
    {
        if (canAttack)
        {
            GetComponentInChildren<BulletSpawnerComponent>().Spawn();
            if (shotSound != null) SoundManager.PlaySound(shotSound);
            timeFromLastAttack = 0f;
            canAttack = false;
        }
    }

    public void Damaged()
    {
        if (damagedSound != null) SoundManager.PlaySound(damagedSound);
    }

    public void Die()
    {
        GetComponent<DestructibleComponent>().StartDestroy();
        if (deathSound != null) SoundManager.PlaySound(deathSound);
        GameManager.GameOver();
    }

    
}
