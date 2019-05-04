

public abstract class CursorStateWithPrevious : CursorState
{
    protected CursorState prevState;

    public void setPrevState(CursorState state)
    {
        prevState = state;
    }

    private void ChangePrevState(CursorState prevState)
    {
        if (prevState != null) this.prevState = prevState;
    }

    public override CursorState HandleDisabledBezier(BezierType type)
    {
        ChangePrevState(prevState.HandleDisabledBezier(type));
        return null;
    }

    public override CursorState HandleCurveCreated(BezierType type)
    {
        ChangePrevState(prevState.HandleCurveCreated(type));
        return null;
    }

    public override CursorState HandleInDefRadar()
    {
        ChangePrevState(prevState.HandleInDefRadar());
        return null;
    }

    public override CursorState HandleOutOfDefRadar()
    {
        ChangePrevState(prevState.HandleOutOfDefRadar());
        return null;
    }
}
