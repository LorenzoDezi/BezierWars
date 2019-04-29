using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RedXCursorState : CursorState
{
    public override void Enter(CursorController cursorController)
    {
        Cursor.SetCursor(cursorController.redIcon, Vector2.zero, CursorMode.Auto);
    }

    public override CursorState HandleCurveCreated(BezierType type)
    {
        return null;
    }

    public override CursorState HandleDisabledBezier(BezierType type)
    {
        if (type == BezierType.Attack) return rCState;
        else return null;
    }

    public override CursorState HandleInDefRadar()
    {
        return rXbCstate;
    }

    public override CursorState HandleOutOfDefRadar()
    {
        return null;
    }
}

