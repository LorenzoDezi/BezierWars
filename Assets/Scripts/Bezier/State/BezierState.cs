using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class BezierState : IBezierState
{

    private BezierSpawner spawner;

    public void Enter(BezierSpawner spawner)
    {
        this.spawner = spawner;
        //TODO Eventuali enter events
    }
    public void Exit()
    {
        //TODO Eventuali exit events
    }
    public IBezierState HandleInput()
    {
        if (Input.GetButtonDown(spawner.SplineSpawnAxis) && spawner.Splines > 0)
            return new SplineState();
        if (Input.GetButtonDown(spawner.DefenseBezierAxisName))
        {
            var nodePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            nodePosition.z = 0;
            var distanceFromNode = Vector3.Distance(
                spawner.transform.position, nodePosition);
            if (distanceFromNode > spawner.DefenseBezierMaxDistance
                || distanceFromNode < spawner.DefenseBezierMinDistance)
                return null;
            HandleClick(BezierType.Defense, nodePosition, 
                spawner.DefenseActiveNodes);

        }
        else if (Input.GetButtonDown(spawner.AttackBezierAxisName))
        {
            var nodePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            nodePosition.z = 0;
            HandleClick(BezierType.Attack, 
                nodePosition, spawner.AttackActiveNodes);
        }
        return null;
    }

    /// <summary>
    /// Check if the user has clicked on a bezier curve in order to remove
    /// it.
    /// </summary>
    /// <param name="inputPosition">Position clicked</param>
    /// <param name="type">The bezier type</param>
    /// <returns>true if the curve is removed, false otherwise</returns>
    private bool RemoveBezierCurve(Vector3 inputPosition, BezierType type)
    {
        var hits = Physics2D.CircleCastAll(inputPosition, 25f, Vector2.up, 0f);
        foreach (var hit in hits)
        {
            if (hit.collider != null)
            {
                var bezBuild = hit.collider.GetComponent<BezierBuilderComponent>();
                if (bezBuild != null && bezBuild.Type == type)
                {
                    hit.collider.GetComponent<HealthComponent>()?.ForceDeath();
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Check if a user has clicked on a bezier node in order to remove it.
    /// </summary>
    /// <param name="clickPosition">Position clicked</param>
    /// <param name="nodeList">the node list</param>
    /// <returns>true if the node is removed, false otherwise</returns>
    private bool RemoveBezierNode(Vector3 clickPosition, List<UnityEngine.GameObject> nodeList)
    {
        int activeNodeAtPosIndex = nodeList.FindIndex(
                    (obj) => Vector3.Distance(obj.transform.position, clickPosition) <= spawner.RemovalNodeSenseDistance);
        if (activeNodeAtPosIndex != -1)
        {
            var activeNode = nodeList[activeNodeAtPosIndex];
            nodeList.RemoveAt(activeNodeAtPosIndex);
            activeNode.GetComponent<IDamageable>().Die();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Handles all the bezier logic for the user click.
    /// </summary>
    /// <param name="type">The bezier curve type</param>
    /// <param name="clickPosition"></param>
    /// <param name="nodeList"></param>
    private void HandleClick(
        BezierType type,
        Vector3 clickPosition,
        List<UnityEngine.GameObject> nodeList
        )
    {
        //TODO: Refactor
        //Preliminar check
        if (RemoveBezierCurve(clickPosition, type)
            || RemoveBezierNode(clickPosition, nodeList) || nodeList.Count >= 3)
        {
            spawner.OnFailNodePlacing.Invoke();
            return;
        }
        if (nodeList.Count == 2 && type == BezierType.Defense
            && CheckForBezierOnPlayer(new Vector3[] {
                nodeList[0].transform.position, nodeList[1].transform.position, clickPosition
            }))
        {
            spawner.OnFailNodePlacing.Invoke();
            return;
        }
        //Proceed to instantiate the node
        var node = BezierType.Attack == type ? 
            spawner.AttackBezNodePrefab : spawner.DefenseBezNodePrefab;
        var instance = UnityEngine.GameObject.Instantiate(node);
        instance.transform.position = clickPosition;
        instance.GetComponent<FollowTargetComponent>()?.SetTargetToFollow(spawner.transform);
        nodeList.Add(instance);
        if (nodeList.Count == 3)
        {
            BuildBezier(type, nodeList);
            SoundManager.PlaySound(spawner.BezierCreatedSound);
        }
    }

    /// <summary>
    /// Build the bezier curve, based on the input.
    /// </summary>
    /// <param name="type">The type of the bezier curve.</param>
    /// <param name="nodeList">The node, aka the control points of the curve.</param>
    protected void BuildBezier(BezierType type, List<UnityEngine.GameObject> nodeList)
    {
        UnityEngine.GameObject bezBuilder = BezierType.Attack == type ? 
            spawner.AttackBezBuilderPrefab : spawner.DefenseBezBuilderPrefab;
        bezBuilder = UnityEngine.GameObject.Instantiate(bezBuilder);
        spawner.OnBezierCreated.Invoke(bezBuilder, type);
        var builderComp = bezBuilder.GetComponent<BezierBuilderComponent>();
        builderComp.Init(nodeList, type);
        builderComp.Disabled.AddListener(GenerateOnBezierDisableCallback(type));
        if (BezierType.Defense == type)
        {
            bezBuilder.GetComponent<FollowTargetComponent>().SetTargetToFollow(spawner.transform);
            nodeList.ForEach((obj) => UnityEngine.GameObject.Destroy(obj.GetComponent<FollowTargetComponent>()));
        }
    }

    /// <summary>
    /// Check if the bezier is crossing the player.
    /// </summary>
    /// <param name="nodes"></param>
    /// <returns>true if the bezier is crossing the player, false otherwise</returns>
    protected bool CheckForBezierOnPlayer(Vector3[] nodes)
    {
        for (int currIndex = 0; currIndex <= spawner.BezierLength; currIndex++)
        {
            Vector3 currentCurvePoint = BezierMath.Bernstein(
                currIndex / (float) spawner.BezierLength, 2, nodes);
            if (Vector3.Distance(currentCurvePoint, spawner.transform.position) 
                < spawner.DefenseBezierMinDistance)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Returns the proper callback based on the curve type.
    /// </summary>
    /// <param name="type">The curve type.</param>
    /// <returns>The callback to execute.</returns>
    protected UnityAction GenerateOnBezierDisableCallback(BezierType type)
    {
        if (type == BezierType.Attack)
            return () => { spawner.AttackActiveNodes.Clear(); };
        else
            return () => { spawner.DefenseActiveNodes.Clear(); };
    }
}

