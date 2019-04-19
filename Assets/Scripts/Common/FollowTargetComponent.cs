using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetComponent : MonoBehaviour
{
    private Transform transformToFollow;
    private Vector3 offset;
    bool isFollowing = false;

    private void Update()
    {
        if (transformToFollow == null || !isFollowing)
            return;
        transform.position = transformToFollow.position + offset;
    }

    public void SetTargetToFollow(Transform transformToFollow)
    {
        this.transformToFollow = transformToFollow;
        this.offset = transform.position - transformToFollow.position;
        isFollowing = true;
    }

    public void StopFollowing()
    {
        isFollowing = false;
    }
}
