using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RedBlueCursorState : CursorState
{
    public override void Enter(CursorController cursorController)
    {
        Cursor.SetCursor(cursorController.redBlueIcon, Vector2.zero, CursorMode.Auto);
    }

    public override CursorState HandleCurveCreated(BezierType type)
    {
        if (type == BezierType.Attack)
            return rXbCstate;
        else
            return rbXCstate;
    }

    public override CursorState HandleOutOfDefRadar()
    {
        return rCState;
    }
}

