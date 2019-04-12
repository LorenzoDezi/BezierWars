using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, Damageable
{
    [Header("Input axis names")]
    [SerializeField]
    private string straightMovementAxis;
    [SerializeField]
    private string orientationAxis;
    [SerializeField]
    private string shootAxis;
    [SerializeField]
    private string boostAxis;

    [Header("Movement parameters")]
    [SerializeField]
    private float maxVelocity;
    [SerializeField]
    private float brakeIntensity = 5f;
    [SerializeField]
    private float enginePower = 5f;
    [SerializeField]
    private float torqueIntensity = 5f;
    [SerializeField]
    private float maxAngularVelocity;
    private bool isAccelerating = true;


    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        float straightMovInput = Input.GetAxis(straightMovementAxis);
        float orientInput = Input.GetAxis(orientationAxis);

        //We use max function because it is used to limit the player speed.
        float currentVelocity = Mathf.Max(
            Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y),
            Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x)
        );
        //Detecting changing of direction: the bool value is intended
        //to perform a "brake" if the player wants to change direction
        //after reaching a certain velocity.
        bool isDirectionChanged = (isAccelerating && straightMovInput < 0)
                || (!isAccelerating && straightMovInput > 0);
        if (isDirectionChanged)
        {
            isAccelerating = !isAccelerating;
            if(currentVelocity >= maxVelocity-0.5f)
                GetComponent<Rigidbody2D>().AddForce(
                    straightMovInput * brakeIntensity * transform.up, ForceMode2D.Impulse);
        }
        else if (straightMovInput != 0 && 
            currentVelocity <= maxVelocity)
        {
            GetComponent<Rigidbody2D>().AddForce(
                straightMovInput * enginePower * transform.up);
        }
            

        if (orientInput != 0 &&
            Mathf.Abs(GetComponent<Rigidbody2D>().angularVelocity) <= maxAngularVelocity)
        {
            GetComponent<Rigidbody2D>().AddTorque(
                orientInput * torqueIntensity);
        }

        if(Input.GetButton(shootAxis))
        {
            GetComponentInChildren<BulletSpawnerComponent>().Spawn();
        }

    }

    public void Damaged()
    {
        Debug.Log("Player damaged!");
        //TODO: Do particle system and shit
    }

    public void Die()
    {
        Debug.Log("Player dead! GAME OVER");
        //TODO: Do particle system and shit
        //TODO: call interface to show game over and restart
    }
}
