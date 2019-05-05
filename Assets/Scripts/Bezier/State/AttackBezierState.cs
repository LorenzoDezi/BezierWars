
using UnityEngine;

public class AttackBezierState : BezierState
{

    public override BezierState HandleInput()
    {
        if (Input.GetButtonDown(spawner.AttackBezierAxisName))
        {
            var nodePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            nodePosition.z = 0;
            return HandleClick(nodePosition);
        }
        return null;       
    }

    public override void Enter(BezierSpawner spawner, CursorComponent cursorComp)
    {
        base.Enter(spawner, cursorComp);
        this.cursorIcon = cursorComp.redIcon;
        SetStateCursor();
    }

    protected BezierState HandleClick(Vector3 clickPosition)
    {
        if (RemoveBezierNode(clickPosition, BezierType.Attack)) return null;
        var nodeList = spawner.NodeListDictionary[BezierType.Attack];
        var node = spawner.NodePrefabs[BezierType.Attack];
        var instance = UnityEngine.GameObject.Instantiate(node);
        instance.transform.position = clickPosition;
        nodeList.Add(instance);
        if (nodeList.Count == 3)
        {
            BuildBezier(BezierType.Attack);
            return BezierState.rmAtkBezState;
        }
        return null;
    }

    public override BezierState OnDefRadarIn()
    {
        return atkDefBezState;
    }
}