using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BezierSpawner : MonoBehaviour
{
    private enum BezierType
    {
        Attack, Defense
    }

    [Header("Input axis")]
    [SerializeField]
    private string mouseXAxisName = "Mouse X";
    [SerializeField]
    private string mouseYAxisName = "Mouse Y";
    [SerializeField]
    private string defenseBezierAxisName = "DefenseBezierSpawn";
    [SerializeField]
    private string attackBezierAxisName = "AttackBezierSpawn";

    [Header("Parameters")]
    [SerializeField]
    private GameObject defenseBezNodePrefab;
    [SerializeField]
    private GameObject attackBezNodePrefab;
    [SerializeField]
    private float defenseBezierMaxDistance;
    [SerializeField]
    private float removalNodeSenseDistance = 1f;
    [SerializeField]
    private int bezierLength = 25;

    private List<GameObject> defActiveNodes = new List<GameObject>();
    private List<GameObject> atkActiveNodes = new List<GameObject>();

    private void Update()
    {
        if(Input.GetButtonDown(defenseBezierAxisName))
        {
            var nodePosition  =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
            nodePosition.z = 0;
            if (Vector3.Distance(transform.position, nodePosition) > defenseBezierMaxDistance)
                return;
            HandleClick(defenseBezNodePrefab, BezierType.Defense, nodePosition, defActiveNodes, transform);

        } else if (Input.GetButtonDown(attackBezierAxisName))
        {
            var nodePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            nodePosition.z = 0;
            HandleClick(attackBezNodePrefab, BezierType.Attack, nodePosition, atkActiveNodes);
        }
    }

    private void HandleClick(
        GameObject node,
        BezierType type,
        Vector3 nodePosition, 
        List<GameObject> list,
        Transform parent = null
        )
    {
        //Preliminar check
        if (list.Count >= 3)
            return;
        //Remove check: if the node is near another already placed, the player intent is to delete it
        int activeNodeAtPosIndex = list.FindIndex(
            (obj) => Vector3.Distance(obj.transform.position, nodePosition) <= removalNodeSenseDistance);
        if (activeNodeAtPosIndex != -1)
        {
            var activeNode = list[activeNodeAtPosIndex];
            list.RemoveAt(activeNodeAtPosIndex);
            activeNode.GetComponent<Damageable>().Die();
            return;
        }
        //Proceed to instantiate the node
        var instance = GameObject.Instantiate(node);
        instance.transform.parent = parent;
        instance.transform.position = nodePosition;
        list.Add(instance);
        if (list.Count == 3)
            BuildCurve(type, list);
        //TODO Handle blocked controls if one curve built
    }

    private void BuildCurve(BezierType type, List<GameObject> list)
    {

        //Build the curve using lineRenderer
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = bezierLength+1;
        for(int currIndex = 0; currIndex <= bezierLength; currIndex++)
        {
            Vector3 currentCurvePoint = BezierMath.Bernstein(
                currIndex / (float) bezierLength, 2, list.ConvertAll((obj) => obj.transform.position)
            );
            lineRenderer.SetPosition(currIndex, currentCurvePoint);
        }
        //Build the collider around the curve
        //1 way:
        //Add all the components needed
        //2 way I THINK THIS IS THE BEST
        //Add a curve gameObject prefab
        //make nodes son of this prefab
        
    }

}
