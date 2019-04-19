using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class BezierBuilderComponent : MonoBehaviour
{
    [SerializeField]
    private int bezierLength = 25;
    private List<GameObject> nodes;
    public UnityEvent Disabled = new UnityEvent();

    public void BuildBezier()
    {
        //PLACEHOLDER - Build the curve using lineRenderer, waiting for particle
        //TODO - Add particle system
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = bezierLength + 1;
        List<Vector2> colliderPositions = new List<Vector2>();
        for (int currIndex = 0; currIndex <= bezierLength; currIndex++)
        {
            Vector3 currentCurvePoint = BezierMath.Bernstein(
                currIndex / (float)bezierLength, 2, nodes.ConvertAll((obj) => obj.transform.position)
            );
            lineRenderer.SetPosition(currIndex, currentCurvePoint);
            colliderPositions.Add(currentCurvePoint);
        }
        EdgeCollider2D collider = GetComponent<EdgeCollider2D>();
        collider.points = colliderPositions.ToArray();
    }

    public void Init(List<GameObject> nodes)
    {
        nodes.ForEach((obj) => {
            obj.transform.parent = transform;
        });
        this.nodes = nodes;
        BuildBezier();
    }
}
