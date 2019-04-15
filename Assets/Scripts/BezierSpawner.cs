using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BezierSpawner : MonoBehaviour
{
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
            HandleClick(defenseBezNodePrefab, nodePosition, defActiveNodes, transform);

        } else if (Input.GetButtonDown(attackBezierAxisName))
        {
            var nodePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            nodePosition.z = 0;
            HandleClick(attackBezNodePrefab, nodePosition, atkActiveNodes);
        }
    }

    private void HandleClick(GameObject node, Vector3 nodePosition, List<GameObject> list, Transform parent = null)
    {
        //TODO: Check node distance before placing, node count, and at the end build the curve.
        int activeNodeAtPosIndex = list.FindIndex(
            (obj) => Vector3.Distance(obj.transform.position, nodePosition) <= removalNodeSenseDistance);
        if (activeNodeAtPosIndex != -1)
        {
            var activeNode = list[activeNodeAtPosIndex];
            list.RemoveAt(activeNodeAtPosIndex);
            activeNode.GetComponent<Damageable>().Die();
            return;
        }
        var instance = GameObject.Instantiate(node);
        //TODO: At the moment, we're attaching the node as son
        instance.transform.parent = parent;
        instance.transform.position = nodePosition;
        list.Add(instance);
    }
}
