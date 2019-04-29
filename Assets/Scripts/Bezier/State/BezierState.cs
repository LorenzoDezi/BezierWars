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
            return new HermiteState();
        if (Input.GetButtonDown(spawner.DefenseBezierAxisName))
        {
            var nodePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            nodePosition.z = 0;
           
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
            activeNode.GetComponent<BezierNodeComponent>().DestroyNode();
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
        if(nodeList.Count == 3)
        {
            spawner.ActiveCurves[type].GetComponent<HealthComponent>()?.ForceDeath();
            return;
        }
        if(type == BezierType.Defense)
        {
            var distanceFromNode = Vector3.Distance(
               spawner.transform.position, clickPosition);
            if (distanceFromNode > spawner.DefenseBezierMaxDistance
                || distanceFromNode < spawner.DefenseBezierMinDistance)
                return;
        }
        if (RemoveBezierNode(clickPosition, nodeList) || nodeList.Count >= 3)
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
        spawner.ActiveCurves.Add(type, bezBuilder);
        var builderComp = bezBuilder.GetComponent<BezierBuilderComponent>();
        builderComp.Init(nodeList, type, spawner.BezierLength);
        builderComp.Disabled.AddListener(OnBezierDisabled);
        if (BezierType.Defense == type)
        {
            bezBuilder.GetComponent<FollowTargetComponent>().SetTargetToFollow(spawner.transform);
            nodeList.ForEach((obj) => UnityEngine.GameObject.Destroy(obj.GetComponent<FollowTargetComponent>()));
        }
    }

    protected void onBezierDisabled(BezierType type)
    {
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
    protected void OnBezierDisabled(BezierType type)
    {
        spawner.ActiveCurves.Remove(type);
        if (type == BezierType.Attack)
            spawner.AttackActiveNodes.Clear();
        else
            spawner.DefenseActiveNodes.Clear();
    }
}

