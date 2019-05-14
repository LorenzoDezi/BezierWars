using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermiteNodeComponent : BezierNodeComponent
{
    private Vector3 tangent;

    private void Start()
    {
        tangent = transform.up;
    }


    public void SetTangent(HermiteNodeComponent nextNode)
    {
        this.tangent = (nextNode.transform.position - transform.position).normalized;
    }

    public Vector3 GetTangent()
    {
        return tangent;
    }
}
