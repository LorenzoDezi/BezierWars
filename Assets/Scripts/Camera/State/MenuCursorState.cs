using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class MenuCursorState : CursorStateWithPrevious
{
    public override void Enter(CursorController cursorController)
    {
        Cursor.SetCursor(cursorController.menuIcon, Vector2.zero, CursorMode.Auto);
    }

    public override CursorState HandleGameStateChange(GameState state)
    {
        if (state != GameState.Menu && state != GameState.Pause && state != GameState.GameOver)
            return prevState;
        else return null;
    }
}
