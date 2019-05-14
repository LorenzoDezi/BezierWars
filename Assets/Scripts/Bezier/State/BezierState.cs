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
    protected CursorComponent cursorComp;
    protected Texture2D cursorIcon;

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

    public virtual void Enter(BezierSpawner spawner, 
        CursorComponent cursorComp)
    {
        this.spawner = spawner;
        this.cursorComp = cursorComp;
    }

    public void SetStateCursor()
    {
#if UNITY_WEBGL
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
#endif
        Cursor.SetCursor(this.cursorIcon, Vector2.zero, CursorMode.Auto);
    }

    public void ResetCursor()
    {
#if UNITY_WEBGL
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
#endif
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public virtual void Exit() { }

    public virtual BezierState SwitchHermite(BezierState previousState)
    {
        if (spawner.CurrHermiteAttempts < spawner.MaxHermiteAttempts)
        {
            hermiteState.NormalCurrentState = previousState;
            spawner.CurrHermiteAttempts++;
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
        spawner.OnBezierCreated.Invoke();
    }

    public virtual BezierState OnGameOver() { return null; }

}

