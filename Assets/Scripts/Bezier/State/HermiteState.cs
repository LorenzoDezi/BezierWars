using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HermiteState : BezierState
{
    private BezierState normalCurrentState;
    private List<GameObject> currentHermiteNodes;
    public BezierState NormalCurrentState {
        get => normalCurrentState; set => normalCurrentState = value; }
    private float currentTimeFromStart;

    public HermiteState()
    {
        currentHermiteNodes = new List<GameObject>();
    }

    public override void Enter(BezierSpawner spawner)
    {
        base.Enter(spawner);
        GameManager.EnterPlacingHermite();
        currentTimeFromStart = 0f;
    }

    public override void Exit()
    {
        GameManager.ExitPlacingHermite();
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
        var instance = UnityEngine.GameObject.Instantiate(node);
        instance.transform.position = clickPosition;
        currentHermiteNodes.Add(instance);
    }

    public override BezierState SwitchHermite(BezierState previousState)
    {
        Debug.Log(previousState + " ---> " + this);
        BuildHermite();
        currentHermiteNodes.ForEach((obj) => obj.GetComponent<HermiteNodeComponent>().Consume());
        currentHermiteNodes = new List<GameObject>();
        return normalCurrentState;
    }

    private void BuildHermite()
    {
        var builder = UnityEngine.GameObject.Instantiate(spawner.HermiteBuilderPrefab);
        var builderComp = builder.GetComponent<HermiteBuilderComponent>();
        builderComp.Init(this.currentHermiteNodes, spawner.BezierLength);
        SoundManager.PlaySound(spawner.BezierCreatedSound);
    }

    public override BezierState OnDefRadarExit()
    {
        normalCurrentState = normalCurrentState?.OnDefRadarExit();
        return null;
    }

    public override BezierState OnDefRadarIn()
    {
        normalCurrentState = normalCurrentState?.OnDefRadarIn();
        return null;
    }
}

