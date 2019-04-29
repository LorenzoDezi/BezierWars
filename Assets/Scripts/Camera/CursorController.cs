using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private static CursorController instance;

    CursorState cursorState;

    [Header("Cursor icons")]
    [SerializeField]
    public Texture2D redIcon;
    [SerializeField]
    public Texture2D redXIcon;
    [SerializeField]
    public Texture2D redBlueIcon;
    [SerializeField]
    public Texture2D redXBlueIcon;
    [SerializeField]
    public Texture2D redBlueXIcon;
    [SerializeField]
    public Texture2D redXBlueXIcon;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        DontDestroyOnLoad(this);
    }

    public static CursorController GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        cursorState = CursorState.rbCState;
        cursorState.Enter(this);
    }

    private void ChangeState(CursorState state)
    {
        Debug.Log(state);
        if (state == null) return;
        state.Enter(this);
        this.cursorState = state;
    }

    public void HandleDisabledBezier(BezierType type)
    {
        CursorState state = cursorState.HandleDisabledBezier(type);
        ChangeState(state);
    }

    public void HandleOutOfDefRadar()
    {
        CursorState state = cursorState.HandleOutOfDefRadar();
        ChangeState(state);
    }

    public void HandleInDefRadar()
    {
        CursorState state = cursorState.HandleInDefRadar();
        ChangeState(state);
    }

    public void HandleCurveCreated(BezierType type)
    {
        CursorState state = cursorState.HandleCurveCreated(type);
        ChangeState(state);
    }
}
