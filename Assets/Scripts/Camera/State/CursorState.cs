﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class CursorState
{
    public static RedBlueCursorState rbCState = new RedBlueCursorState();
    public static RedCursorState rCState = new RedCursorState();
    public static RedBlueXCursorState rbXCstate = new RedBlueXCursorState();
    public static RedXBlueCursorState rXbCstate = new RedXBlueCursorState();
    public static RedXBlueXCursorState rXbXCstate = new RedXBlueXCursorState();
    public static RedXCursorState rXCstate = new RedXCursorState();
    public static MenuCursorState menuState = new MenuCursorState();
    public static HermiteCursorState hermiteState = new HermiteCursorState();

    protected Texture2D cursorTexture;

    public abstract void Enter(CursorController camera);
    public virtual CursorState HandleDisabledBezier(BezierType type) { return null; }
    public virtual CursorState HandleOutOfDefRadar() { return null; }
    public virtual CursorState HandleInDefRadar() { return null; }
    public virtual CursorState HandleCurveCreated(BezierType type) { return null; }
    public virtual CursorState HandleGameStateChange(GameState state) {
        if (GameState.Menu == state || GameState.Pause == state || GameState.GameOver == state)
            return CursorState.menuState;
        else if (GameState.PlacingSpline == state)
            return CursorState.hermiteState;
        else return null;
    }
}

