using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour, IEnemyController
{

    [SerializeField]
    private float engineIntensity = 5f;

    public void Move(Vector2 direction, float speedFactor)
    {
        GetComponent<Rigidbody2D>().AddForce(direction * speedFactor * engineIntensity,
            ForceMode2D.Force);
    }

}
