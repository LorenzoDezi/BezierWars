using UnityEngine;

public class RemoveAttackRemoveDefenseBezierState : BezierState
{
    bool inDefRadar = false;

    public override BezierState HandleInput()
    {
        if (Input.GetButtonDown(spawner.AttackBezierAxisName))
        {
            RemoveBezier(BezierType.Attack);
            return atkRmDefBezState;
        }
        if (Input.GetButtonDown(spawner.DefenseBezierAxisName))
        {
            RemoveBezier(BezierType.Defense);
            if (inDefRadar) return rmAtkDefBezState;
            else return rmAtkBezState;
        }
        return null;
    }

    public override BezierState OnBezierDisabled(BezierType type)
    {
        if (type == BezierType.Attack)
            return atkRmDefBezState;
        else
            return rmAtkDefBezState;
    }

    public override BezierState OnDefRadarExit()
    {
        inDefRadar = false;
        return null;
    }

    public override BezierState OnDefRadarIn()
    {
        inDefRadar = true;
        return null;
    }
}