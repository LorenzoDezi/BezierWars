using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RedCursorState : CursorState
{
    public override void Enter(CursorController cursorController)
    {
        Cursor.SetCursor(cursorController.redIcon, Vector2.zero, CursorMode.Auto);
    }

    public override CursorState HandleCurveCreated(BezierType type)
    {
        return type == BezierType.Attack ? rXCstate : null;
    }

    public override CursorState HandleInDefRadar()
    {
        return rbCState;
    }
}

