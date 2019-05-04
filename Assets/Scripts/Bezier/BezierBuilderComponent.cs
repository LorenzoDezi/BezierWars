using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;


public class DisabledEvent : UnityEvent<BezierType>
{

}

public class CreatedEvent : UnityEvent<BezierType>
{

}

[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class BezierBuilderComponent : MonoBehaviour
{
    private int bezierLength;

    private BezierType type;
    private List<UnityEngine.GameObject> nodes;

    public DisabledEvent Disabled;
    public CreatedEvent Created;
    

    public BezierType Type => type;

    private void Awake()
    {
        Disabled = new DisabledEvent();
        Created = new CreatedEvent();
        Disabled.AddListener(CursorController.GetInstance().HandleDisabledBezier);
        Created.AddListener(CursorController.GetInstance().HandleCurveCreated);
    }

    private void Start()
    {
        //I instantiate the actual material, in order to modify its properties
        //runtime.
        var lineRendererMat = GetComponent<LineRenderer>().material;
        lineRendererMat = Instantiate(
            lineRendererMat);
    }
    

    IEnumerator BuildBezier(Vector3[] nodes)
    {
        var lineRenderer = GetComponent<LineRenderer>();
        var collider = GetComponent<EdgeCollider2D>();
        List<Vector2> linePoints = new List<Vector2>();
        for (int currIndex = 0; currIndex <= bezierLength; currIndex++)
        {
            Vector3 currentCurvePoint = BezierMath.BernsteinBezier(
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

    public void Init(List<UnityEngine.GameObject> nodes, BezierType type, int bezierLength)
    {
        nodes.ForEach((obj) => {
            obj.transform.parent = transform;
        });
        this.nodes = nodes;
        this.type = type;
        this.bezierLength = bezierLength;
        Created.Invoke(type);
        StartCoroutine(BuildBezier(
            nodes.ConvertAll((n) => n.GetComponent<Transform>().position).ToArray()));
    }
}
