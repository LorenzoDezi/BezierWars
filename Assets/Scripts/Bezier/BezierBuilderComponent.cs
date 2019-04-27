using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class BezierBuilderComponent : MonoBehaviour
{
    [SerializeField]
    private int bezierLength = 25;
    [SerializeField]
    private float bezierBuildTimeStep;

    private BezierType type;
    private List<UnityEngine.GameObject> nodes;

    public UnityEvent Disabled;
    

    public BezierType Type => type;

    private void Awake()
    {
        Disabled = new UnityEvent();
    }

    public void BuildBezier()
    {
        
    }

    IEnumerator BuildBezier(Vector3[] nodes)
    {
        var lineRenderer = GetComponent<LineRenderer>();
        var collider = GetComponent<EdgeCollider2D>();
        List<Vector2> linePoints = new List<Vector2>();
        for (int currIndex = 0; currIndex <= bezierLength; currIndex++)
        {
            Vector3 currentCurvePoint = BezierMath.Bernstein(
                currIndex / (float)bezierLength, 2,
                nodes
            );
            linePoints.Add(currentCurvePoint);
        }
        collider.points = linePoints.ToArray();
        //The second for loop will render the actual line. This is done to build
        //the collider before the actual rendering
        for(int i = 0; i <= bezierLength; i++)
        {
            lineRenderer.positionCount = i + 1;
            lineRenderer.SetPosition(i, linePoints[i]);
            yield return null;
        }

    }

    public void Init(List<UnityEngine.GameObject> nodes, BezierType type)
    {
        nodes.ForEach((obj) => {
            obj.transform.parent = transform;
        });
        this.nodes = nodes;
        this.type = type;
        StartCoroutine(BuildBezier(
            nodes.ConvertAll((n) => n.GetComponent<Transform>().position).ToArray()));
    }
}
