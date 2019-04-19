using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BezierCreatedEvent : UnityEvent<GameObject, BezierType> { }

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

    [Header("Prefabs")]
    [SerializeField]
    private UnityEngine.GameObject defenseBezNodePrefab;
    [SerializeField]
    private UnityEngine.GameObject attackBezNodePrefab;
    [SerializeField]
    private UnityEngine.GameObject attackBezBuilderPrefab;
    [SerializeField]
    private UnityEngine.GameObject defenseBezBuilderPrefab;

    [Header("Parameters")]
    [SerializeField]
    private float defenseBezierMaxDistance;
    [SerializeField]
    private float removalNodeSenseDistance = 1f;

    private List<UnityEngine.GameObject> defenseActiveNodes = new List<UnityEngine.GameObject>();
    private List<UnityEngine.GameObject> attackActiveNodes = new List<UnityEngine.GameObject>();
    private BezierCreatedEvent onBezierCreated;

    public BezierCreatedEvent OnBezierCreated => onBezierCreated;

    private void Awake()
    {
        onBezierCreated = new BezierCreatedEvent();
    }

    private void Update()
    {
        if(Input.GetButtonDown(defenseBezierAxisName))
        {
            var nodePosition  =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
            nodePosition.z = 0;
            if (Vector3.Distance(transform.position, nodePosition) > defenseBezierMaxDistance)
                return;
            HandleClick(defenseBezNodePrefab, BezierType.Defense, nodePosition, defenseActiveNodes, transform);

        } else if (Input.GetButtonDown(attackBezierAxisName))
        {
            var nodePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            nodePosition.z = 0;
            HandleClick(attackBezNodePrefab, BezierType.Attack, nodePosition, attackActiveNodes);
        }
    }

    private UnityAction GenerateOnBezierDisableCallback(BezierType type)
    {
        if (type == BezierType.Attack)
            return () => { this.attackActiveNodes.Clear(); };
        else
            return () => { this.defenseActiveNodes.Clear(); };
    }




    private void HandleClick(
        UnityEngine.GameObject node,
        BezierType type,
        Vector3 nodePosition, 
        List<UnityEngine.GameObject> list,
        Transform parent = null
        )
    {
        //Preliminar check
        if (list.Count >= 3)
            return;
        //Remove check: if the node is near another already placed, the player intent is to delete it
        CheckForNodeRemoval(nodePosition, list);
        //Proceed to instantiate the node
        var instance = UnityEngine.GameObject.Instantiate(node);
        instance.transform.position = nodePosition;
        instance.GetComponent<FollowTargetComponent>()?.SetTargetToFollow(transform);
        list.Add(instance);
        if (list.Count == 3)
        {
            BuildBezier(type, list);
        }
    }

    private void BuildBezier(BezierType type, List<UnityEngine.GameObject> list)
    {
        UnityEngine.GameObject bezBuilder = BezierType.Attack == type ? attackBezBuilderPrefab : defenseBezBuilderPrefab;
        bezBuilder = UnityEngine.GameObject.Instantiate(bezBuilder);
        onBezierCreated.Invoke(bezBuilder, type);
        var builderComp = bezBuilder.GetComponent<BezierBuilderComponent>();
        builderComp.Init(list);
        builderComp.Disabled.AddListener(GenerateOnBezierDisableCallback(type));
        if (BezierType.Defense == type)
        {
            bezBuilder.GetComponent<FollowTargetComponent>().SetTargetToFollow(transform);
            list.ForEach((obj) => UnityEngine.GameObject.Destroy(obj.GetComponent<FollowTargetComponent>()));
        }
    }

    private void CheckForNodeRemoval(Vector3 nodePosition, List<UnityEngine.GameObject> list)
    {
        int activeNodeAtPosIndex = list.FindIndex(
                    (obj) => Vector3.Distance(obj.transform.position, nodePosition) <= removalNodeSenseDistance);
        if (activeNodeAtPosIndex != -1)
        {
            var activeNode = list[activeNodeAtPosIndex];
            list.RemoveAt(activeNodeAtPosIndex);
            activeNode.GetComponent<IDamageable>().Die();
        }
    }
}
