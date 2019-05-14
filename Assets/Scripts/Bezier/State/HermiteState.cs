using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HermiteState : BezierState
{
    private BezierState normalCurrentState;
    private List<HermiteNodeComponent> currentHermiteNodes;
    public BezierState NormalCurrentState {
        get => normalCurrentState; set => normalCurrentState = value; }
    private float currentTimeFromStart;

    public HermiteState()
    {
        currentHermiteNodes = new List<HermiteNodeComponent>();
    }

    public override void Enter(BezierSpawner spawner, CursorComponent cursorComp)
    {
        base.Enter(spawner, cursorComp);
        this.cursorIcon = cursorComp.hermiteIcon;
        currentHermiteNodes = new List<HermiteNodeComponent>();
        SetStateCursor();
        GameManager.EnterPlacingHermite();
        spawner.EnteredHermiteMode.Invoke();
        currentTimeFromStart = 0f;
    }

    public override void Exit()
    {
        GameManager.ExitPlacingHermite();
        spawner.ExitedHermiteMode.Invoke();
    }

    public override BezierState OnGameOver()
    {
        return SwitchHermite(null);
    }

    public override BezierState HandleInput()
    {
        currentTimeFromStart += Time.deltaTime;
        if (currentTimeFromStart >= spawner.HermitePlacingTimeLimit)
            return SwitchHermite(null);
        if(Input.GetButtonDown(spawner.DefenseBezierAxisName) || Input.GetButtonDown(spawner.AttackBezierAxisName))
        {
            var nodePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            nodePosition.z = 0;
            if(!currentHermiteNodes.Any((node) => node.GetComponent<Collider2D>().OverlapPoint(nodePosition)))
                HandleClick(nodePosition);
        }
        return null;
    }

    private void HandleClick(Vector3 clickPosition)
    {
        var node = spawner.HermiteNodePrefab;
        var instance = UnityEngine.GameObject.Instantiate(node).GetComponent<HermiteNodeComponent>();
        instance.transform.position = clickPosition;
        if(currentHermiteNodes.Count > 0)
            currentHermiteNodes[currentHermiteNodes.Count-1]
                .GetComponent<HermiteNodeComponent>().SetTangent(instance);
        currentHermiteNodes.Add(instance);
    }

    public override BezierState SwitchHermite(BezierState previousState)
    {
        BuildHermite();
        currentHermiteNodes = new List<HermiteNodeComponent>();
        return normalCurrentState;
    }

    private void BuildHermite()
    {
        var builder = UnityEngine.GameObject.Instantiate(spawner.HermiteBuilderPrefab);
        var builderComp = builder.GetComponent<HermiteBuilderComponent>();
        //The last hermite node will have the previous node tangent
        if (currentHermiteNodes.Count > 1)
            this.currentHermiteNodes[currentHermiteNodes.Count - 1]
                .SetTangent(currentHermiteNodes[currentHermiteNodes.Count - 2]);
        builderComp.Init(this.currentHermiteNodes, spawner.BezierLength);
        SoundManager.PlaySound(spawner.BezierCreatedSound);
    }

    public override BezierState OnBezierDisabled(BezierType type)
    {
        ChangeNormalCurrentState(normalCurrentState.OnBezierDisabled(type));
        return null;
    }

    private void ChangeNormalCurrentState(BezierState newState)
    {
        if (newState != null)
            normalCurrentState = newState;
    }

    public override BezierState OnDefRadarExit()
    {
        ChangeNormalCurrentState(normalCurrentState.OnDefRadarExit());
        return null;
    }

    public override BezierState OnDefRadarIn()
    {
        ChangeNormalCurrentState(normalCurrentState.OnDefRadarIn());
        return null;
    }
}

