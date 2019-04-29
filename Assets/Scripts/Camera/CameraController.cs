using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform transformToFollow;
    [SerializeField]
    private float smoothSpeed = 0.0125f;
    [SerializeField]
    private Vector3 offset;

    public void SetTransformToFollow(Transform transform)
    {
        this.transformToFollow = transform;
    }

    private void FixedUpdate()
    {
        if (transformToFollow == null) return;
        Vector3 desiredPosition = transformToFollow.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.fixedDeltaTime);
        transform.position = smoothedPosition;
    }

    
}
