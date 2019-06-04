using UnityEngine;

public class AttackRemoveDefenseBezierState : BezierState
{
    public override void Enter(BezierSpawner spawner, CursorComponent cursorComp)
    {
        base.Enter(spawner, cursorComp);
        this.cursorIcon = cursorComp.redBlueXIcon;
        SetStateCursor();
    }

    public override BezierState HandleInput()
    {
        if (Input.GetButtonDown(spawner.DefenseBezierAxisName))
        {
            RemoveBezier(BezierType.Defense);
            return null;
        }
        else if (Input.GetButtonDown(spawner.AttackBezierAxisName))
        {
            var nodePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            nodePosition.z = 0;
            return HandleClick(nodePosition);
        }
        else
            return null;        
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
            return BezierState.rmAtkRmDefBezState;
        }
        return null;
    }

    public override BezierState OnBezierDisabled(BezierType type)
    {
        if (type == BezierType.Defense)
            if (spawner.OnDefRadar)
                return atkDefBezState;
            else
                return atkBezState;
        return null;
    }
}