using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public abstract class BezierState
{
    protected BezierSpawner spawner;

    protected static AttackDefenseBezierState atkDefBezState = new AttackDefenseBezierState();
    protected static RemoveAttackRemoveDefenseBezierState rmAtkRmDefBezState = new RemoveAttackRemoveDefenseBezierState();
    protected static RemoveAttackDefenseBezierState rmAtkDefBezState = new RemoveAttackDefenseBezierState();
    protected static AttackRemoveDefenseBezierState atkRmDefBezState = new AttackRemoveDefenseBezierState();
    protected static AttackBezierState atkBezState = new AttackBezierState();
    protected static RemoveAttackBezierState rmAtkBezState = new RemoveAttackBezierState();
    protected static HermiteState hermiteState = new HermiteState();

    public static BezierState GetInitalState()
    {
        return atkDefBezState;
    }

    public abstract BezierState HandleInput();

    public virtual void Enter(BezierSpawner spawner)
    {
        this.spawner = spawner;
    }

    public virtual void Exit() { }

    public virtual BezierState SwitchHermite(BezierState previousState)
    {
        Debug.Log(previousState + " ---> " + this);
        if (spawner.CurrHermiteAttempts > 0)
        {
            hermiteState.NormalCurrentState = previousState;
            spawner.CurrHermiteAttempts--;
            return hermiteState;
        }
        else return null;
    }

    public virtual BezierState OnDefRadarExit()
    {
        return null;
    }

    public virtual BezierState OnDefRadarIn()
    {
        return null;
    }

    public virtual BezierState OnBezierDisabled(BezierType type)
    {
        return null;
    }

    protected void RemoveBezier(BezierType type)
    {
        spawner.ActiveCurves[type].GetComponent<HealthComponent>()?.ForceDeath();
    }

    protected bool RemoveBezierNode(Vector3 clickPosition,
        BezierType type)
    {
        var nodeList = spawner.NodeListDictionary[type];
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

    protected bool CheckBezierCrossingSpawner(Vector3[] nodes)
    {
        for (int currIndex = 0; currIndex <= spawner.BezierLength; currIndex++)
        {
            Vector3 currentCurvePoint = BezierMath.BernsteinBezier(
                currIndex / (float)spawner.BezierLength, 2, nodes);
            if (Vector3.Distance(currentCurvePoint, spawner.transform.position)
                < spawner.DefenseBezierMinDistance)
                return true;
        }
        return false;
    }

    protected void BuildBezier(BezierType type)
    {
        UnityEngine.GameObject bezBuilder = spawner.BuilderPrefabs[type];
        var nodeList = spawner.NodeListDictionary[type];
        bezBuilder = UnityEngine.GameObject.Instantiate(bezBuilder);
        spawner.ActiveCurves.Add(type, bezBuilder);
        var builderComp = bezBuilder.GetComponent<BezierBuilderComponent>();
        builderComp.Init(nodeList, type, spawner.BezierLength);
        builderComp.Disabled.AddListener(spawner.OnBezierDisabled);
        if (BezierType.Defense == type)
        {
            bezBuilder.GetComponent<FollowTargetComponent>().SetTargetToFollow(spawner.transform);
            nodeList.ForEach((obj) => UnityEngine.GameObject.Destroy(obj.GetComponent<FollowTargetComponent>()));
        }
        SoundManager.PlaySound(spawner.BezierCreatedSound);
    }

}

