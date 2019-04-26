using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(PostProcessVolume))]
public class BezierBuilderComponent : MonoBehaviour
{
    [SerializeField]
    private int bezierLength = 25;
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
        //PLACEHOLDER - Build the curve using lineRenderer, waiting for particle
        //TODO - Add particle system
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        Bloom bloomLayer;
        GetComponent<PostProcessVolume>().profile.TryGetSettings(out bloomLayer);
        bloomLayer.color.Override(lineRenderer.startColor);
        lineRenderer.positionCount = bezierLength + 1;
        List<Vector2> colliderPositions = new List<Vector2>();
        for (int currIndex = 0; currIndex <= bezierLength; currIndex++)
        {
            Vector3 currentCurvePoint = BezierMath.Bernstein(
                currIndex / (float)bezierLength, 2, 
                nodes.ConvertAll((obj) => obj.transform.position).ToArray()
            );
            lineRenderer.SetPosition(currIndex, currentCurvePoint);
            colliderPositions.Add(currentCurvePoint);
        }
        EdgeCollider2D collider = GetComponent<EdgeCollider2D>();
        collider.points = colliderPositions.ToArray();
    }

    public void Init(List<UnityEngine.GameObject> nodes, BezierType type)
    {
        nodes.ForEach((obj) => {
            obj.transform.parent = transform;
        });
        this.nodes = nodes;
        this.type = type;
        BuildBezier();
    }
}
