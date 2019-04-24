using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineSpawner : MonoBehaviour
{
    [SerializeField]
    private string splineSpawnAxis = "SplineSpawn";
    [SerializeField]
    private string defenseSplineSpawn = "DefenseBezierSpawn";
    [SerializeField]
    private string attackSplineSpawn = "AttackBezierSpawn";

    private bool isPlacingSpline = false;
    [SerializeField]
    private float timeToPlace = 2f;
    private float startPlacingTime = float.NegativeInfinity;

    private void Update()
    {
        if(Input.GetButtonDown(splineSpawnAxis))
        {
            isPlacingSpline = true;
            startPlacingTime = Time.time;
            GameManager.EnterPlacingSpline();
        }
        if (!isPlacingSpline) return;
        if(Time.time > timeToPlace + startPlacingTime)
        {
            isPlacingSpline = false;
            GameManager.ExitPlacingSpline();
            return;
        }
        if(Input.GetButtonDown(defenseSplineSpawn))
        {
            //TODO Place a defense spline node 

        } else if (Input.GetButtonDown(attackSplineSpawn))
        {
            //TODO Place an attack spline node
        }


    }
}
