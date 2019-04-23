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
    private string shootAxis;
    [SerializeField]
    private string defenseBezierAxis;
    [SerializeField]
    private string attackBezierAxis;
    [Header("Linked components")]
    [SerializeField]
    private BezierSpawner bezSpawner;


    private void HandleInput()
    {
        float straightMovInput = Input.GetAxis(straightMovementAxis);
        float orientInput = Input.GetAxis(orientationAxis);
        var playerController = GetComponent<PlayerController>();
        if(straightMovInput != 0) playerController.Move(straightMovInput);
        if (orientInput != 0) playerController.Rotate(orientInput);
        if (Input.GetButton(shootAxis))
            playerController.Attack();
        if (bezSpawner == null) return;
        if (Input.GetButtonDown(defenseBezierAxis))
            bezSpawner.PlaceDefenseNode();
        if (Input.GetButtonDown(attackBezierAxis))
            bezSpawner.PlaceAttackNode();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }
}
