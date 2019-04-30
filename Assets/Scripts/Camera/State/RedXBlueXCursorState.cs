﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RedXBlueXCursorState : CursorState
{
    public override void Enter(CursorController cursorController)
    {
        Cursor.SetCursor(cursorController.redXBlueXIcon, Vector2.zero, CursorMode.Auto);
    }

    public override CursorState HandleCurveCreated(BezierType type)
    {
        return null;
    }

    public override CursorState HandleDisabledBezier(BezierType type)
    {
        if (type == BezierType.Attack) return rbXCstate;
        else return rXbCstate;
    }

    public override CursorState HandleInDefRadar()
    {
        return null;
    }

    public override CursorState HandleOutOfDefRadar()
    {
        return null;
    }
}
