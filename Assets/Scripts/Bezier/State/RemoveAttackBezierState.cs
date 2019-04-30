using UnityEngine;

public class RemoveAttackBezierState : BezierState
{
    public override BezierState HandleInput()
    {
        if (Input.GetButtonDown(spawner.SplineSpawnAxis) && spawner.Splines > 0)
            return hermiteState;
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