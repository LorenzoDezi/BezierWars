using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RedXBlueCursorState : CursorState
{
    public override void Enter(CursorController cursorController)
    {
        Cursor.SetCursor(cursorController.redXBlueIcon, Vector2.zero, CursorMode.Auto);
    }

    public override CursorState HandleCurveCreated(BezierType type)
    {
        if (type == BezierType.Defense) return rXbXCstate;
        else return null;
    }

    public override CursorState HandleDisabledBezier(BezierType type)
    {
        if (type == BezierType.Attack) return rbCState;
        else return null;
    }

    public override CursorState HandleOutOfDefRadar()
    {
        return rXCstate;
    }
}

