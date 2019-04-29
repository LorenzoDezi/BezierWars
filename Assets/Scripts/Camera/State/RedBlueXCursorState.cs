using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RedBlueXCursorState : CursorState
{

    bool isOutOfRadar = false;

    public override void Enter(CursorController cursorController)
    {
        Cursor.SetCursor(cursorController.redBlueXIcon, Vector2.zero, CursorMode.Auto);
    }

    public override CursorState HandleCurveCreated(BezierType type)
    {
        return type == BezierType.Attack ? rXbXCstate : null;
    }

    public override CursorState HandleDisabledBezier(BezierType type)
    {
        if (type == BezierType.Defense)
            if (isOutOfRadar) return rCState;
            else return rbCState;
        else return null;
    }

    public override CursorState HandleInDefRadar()
    {
        isOutOfRadar = false;
        return null;
    }

    public override CursorState HandleOutOfDefRadar()
    {
        isOutOfRadar = true;
        return null;
    }
}

