using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RedBlueXCursorState : CursorState
{
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
        return type == BezierType.Defense ? rbCState : null;
    }

    public override CursorState HandleInDefRadar()
    {
        return null;
    }

    public override CursorState HandleOutOfDefRadar()
    {
        return rCState;
    }
}

