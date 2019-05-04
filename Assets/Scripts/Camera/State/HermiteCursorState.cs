using UnityEngine;

public class HermiteCursorState : CursorStateWithPrevious
{
    public override void Enter(CursorController camera)
    {
        Cursor.SetCursor(camera.hermiteIcon, Vector2.zero, CursorMode.Auto);
    }

    public override CursorState HandleGameStateChange(GameState state)
    {
        if (GameState.PlacingSpline != state)
            return prevState;
        else
            return null;
    }
}
