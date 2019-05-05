using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInputComponent : MonoBehaviour
{
    [Header("Input axis names")]
    [SerializeField]
    private string straightMovementAxis;
    [SerializeField]
    private string orientationAxis;
    [SerializeField]
    private string hermiteAxis;
    [SerializeField]
    private string shootAxis;

    [Header("Components")]
    [SerializeField]
    private BezierSpawner spawner;


    private void HandleInput()
    {
        float straightMovInput = Input.GetAxis(straightMovementAxis);
        float orientInput = Input.GetAxis(orientationAxis);
        var playerController = GetComponent<PlayerController>();
        if(straightMovInput != 0) playerController.Move(straightMovInput);
        if (orientInput != 0) playerController.Rotate(orientInput);
        if (Input.GetButton(shootAxis))
            playerController.Attack();
        if (spawner != null)
        {
            spawner.HandleInput();
            if (Input.GetButtonDown(hermiteAxis))
                spawner.SwitchHermite();
        }
    }

    public void SetBezierSpawner(BezierSpawner spawner)
    {
        this.spawner = spawner;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }
}
