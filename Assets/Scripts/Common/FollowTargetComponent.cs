using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetComponent : MonoBehaviour
{
    [SerializeField]
    public Transform transformToFollow;
    private Vector3 offset;

    private void Update()
    {
        if (transformToFollow == null)
            return;
        transform.position = transformToFollow.position + offset;
    }

    public void SetTargetToFollow(Transform transformToFollow)
    {
        this.transformToFollow = transformToFollow;
        this.offset = transform.position - transformToFollow.position;
    }

    public void StopFollowing()
    {
        this.transformToFollow = null;
    }
}
