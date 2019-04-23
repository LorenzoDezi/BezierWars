using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform transformToFollow;
    [SerializeField]
    private float smoothSpeed = 0.0125f;
    [SerializeField]
    private Vector3 offset;

    private void Start()
    {
        GameManager.OnGameStateChange().AddListener(onGameStateChange);
    }

    private void FixedUpdate()
    {
        if (transformToFollow == null) return;
        Vector3 desiredPosition = transformToFollow.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.fixedDeltaTime);
        transform.position = smoothedPosition;
    }

    private void onGameStateChange(GameState state)
    {
        GetComponent<Animator>().SetTrigger(state.ToString());
    }
}