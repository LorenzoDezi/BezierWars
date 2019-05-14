using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class HermiteBuilderComponent : MonoBehaviour
{
    private int bezierLength;
    private List<HermiteNodeComponent> nodes;
    [SerializeField]
    private float tangentSize = 1f;


    IEnumerator BuildHermite(HermiteNodeComponent[] nodes)
    {
        var lineRenderer = GetComponent<LineRenderer>();
        var collider = GetComponent<EdgeCollider2D>();
        List<Vector2> linePoints = new List<Vector2>();
        for (int i = 0; i < nodes.Length - 1; i++)
        {       
            linePoints.AddRange(TwoPointInterpolation(nodes[i], nodes[i+1]));
        }
        collider.points = linePoints.ToArray();
        //The second for loop will render the actual line. This is done to build
        //the collider before the actual rendering
        for(int i = 0; i < linePoints.Count; i++)
        {
            lineRenderer.positionCount = i + 1;
            lineRenderer.SetPosition(i, linePoints[i]);
            if(i % 2 == 0)
                yield return null;
        }
    }

    private List<Vector2> TwoPointInterpolation(HermiteNodeComponent first, HermiteNodeComponent second)
    {
        Vector3 m1 = first.GetTangent() * tangentSize;
        Vector3 p0, p3;
        p0 = first.transform.position;
        p3 = second.transform.position;
        Vector3 m2 = second.GetTangent() * tangentSize;
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        List<Vector2> linePoints = new List<Vector2>();
        for (int currIndex = 0; currIndex <= bezierLength; currIndex++)
        {
            float t = currIndex / (float)bezierLength;
            Vector3 newPoint =
                (BezierMath.BernsteinPolynomial(0, 3, t) + BezierMath.BernsteinPolynomial(1, 3, t)) * p0 +
                (BezierMath.BernsteinPolynomial(3, 3, t) + BezierMath.BernsteinPolynomial(2, 3, t)) * p3 +
                BezierMath.BernsteinPolynomial(1, 3, t) * 1 / 3 * m1 -
                BezierMath.BernsteinPolynomial(2, 3, t) * 1 / 3 * m2;
            linePoints.Add(newPoint);
        }
        return linePoints;
    }

    public void Init(List<HermiteNodeComponent> nodes, int bezierLength)
    {
        nodes.ForEach((obj) => {
            obj.transform.parent = transform;
        });
        this.nodes = nodes;
        this.bezierLength = bezierLength;
        StartCoroutine(BuildHermite(
            nodes.ToArray()));
    }

    
}
