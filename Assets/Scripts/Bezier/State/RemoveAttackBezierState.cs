using UnityEngine;

public class RemoveAttackBezierState : BezierState
{
    public override void Enter(BezierSpawner spawner, CursorComponent cursorComp)
    {
        base.Enter(spawner, cursorComp);
        this.cursorIcon = cursorComp.redXIcon;
        SetStateCursor();
    }

    public override BezierState HandleInput()
    {
        if (Input.GetButtonDown(spawner.AttackBezierAxisName))
        {
            RemoveBezier(BezierType.Attack);
            return atkBezState;
        }
        return null;
    }

    public override BezierState OnBezierDisabled(BezierType type)
    {
        if (type == BezierType.Attack)
            return atkBezState;
        return null;
    }

    public override BezierState OnDefRadarIn()
    {
        return rmAtkDefBezState;
    }


}