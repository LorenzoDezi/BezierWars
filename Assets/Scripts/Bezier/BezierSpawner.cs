using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(FollowTargetComponent))]
public class BezierSpawner : MonoBehaviour
{

    [Header("Input axis")]
    [SerializeField]
    private string defenseBezierAxisName = "DefenseBezierSpawn";
    [SerializeField]
    private string attackBezierAxisName = "AttackBezierSpawn";

    [Header("Prefabs")]
    [SerializeField]
    private GameObject defenseNodePrefab;
    [SerializeField]
    private GameObject atkNodePrefab;
    [SerializeField]
    private GameObject hermiteNodePrefab;
    [SerializeField]
    private GameObject defBuilderPrefab;
    [SerializeField]
    private GameObject atkBuilderPrefab;
    [SerializeField]
    private GameObject hermiteBuilderPrefab;

    [Header("Parameters")]
    [SerializeField]
    private int bezierLength = 25;
    [SerializeField]
    private float hermitePlacingTimeLimit = 5f;
    [SerializeField]
    private float defenseBezierMinDistance = 2f;
    [SerializeField]
    private float removalNodeSenseDistance = 2f;
    [SerializeField]
    private int maxHermiteAttempts = 3;
    private int currHermiteAttempts;

    [Header("Sound effects")]
    [SerializeField]
    private AudioClip bezierCreatedSound;

    [SerializeField]
    private AudioClip failNodeSound;

    private BezierState state;
    private UnityEvent enteredDefRadar;
    private UnityEvent enteredHermiteMode;
    private UnityEvent exitHermiteMode;
    private UnityEvent outOfDefRadar;

    public string AttackBezierAxisName { get => attackBezierAxisName; }
    public string DefenseBezierAxisName { get => defenseBezierAxisName; }
    public float RemovalNodeSenseDistance { get => removalNodeSenseDistance; }
    
    public Dictionary<BezierType, GameObject> NodePrefabs { get; private set; }
    public Dictionary<BezierType, GameObject> BuilderPrefabs { get; private set; }
    public Dictionary<BezierType, List<GameObject>> NodeListDictionary { get; private set; }
    public int CurrHermiteAttempts { get => currHermiteAttempts; set => currHermiteAttempts = value; }
    public UnityEvent EnteredHermiteMode { get => enteredHermiteMode; }
    public UnityEvent ExitedHermiteMode { get => exitHermiteMode;  }
    public int MaxHermiteAttempts { get => maxHermiteAttempts; }
    public float HermitePlacingTimeLimit { get => hermitePlacingTimeLimit; }
    public GameObject HermiteBuilderPrefab { get => hermiteBuilderPrefab; }
    public AudioClip BezierCreatedSound { get => bezierCreatedSound; }
    public int BezierLength { get => bezierLength; }
    public Dictionary<BezierType, GameObject> ActiveCurves { get; private set; }
    public GameObject HermiteNodePrefab => hermiteNodePrefab;
    public float DefenseBezierMinDistance { get => defenseBezierMinDistance; }

    private void Awake()
    {
        //Init fields
        enteredDefRadar = new UnityEvent();
        outOfDefRadar = new UnityEvent();
        enteredHermiteMode = new UnityEvent();
        exitHermiteMode = new UnityEvent();
        ActiveCurves = new Dictionary<BezierType, GameObject>();
        NodeListDictionary = new Dictionary<BezierType, List<GameObject>>();
        NodeListDictionary.Add(BezierType.Attack, new List<GameObject>());
        NodeListDictionary.Add(BezierType.Defense, new List<GameObject>());
        NodePrefabs = new Dictionary<BezierType, GameObject>();
        NodePrefabs.Add(BezierType.Attack, atkNodePrefab);
        NodePrefabs.Add(BezierType.Defense, defenseNodePrefab);
        BuilderPrefabs = new Dictionary<BezierType, GameObject>();
        BuilderPrefabs.Add(BezierType.Attack, atkBuilderPrefab);
        BuilderPrefabs.Add(BezierType.Defense, defBuilderPrefab);
        currHermiteAttempts = 0;
        GameManager.OnGameStateChange().AddListener(ChangeMaxHermiteAttempts);

        //Init state
        state = BezierState.GetInitalState();
        state.Enter(this, GetComponent<CursorComponent>());
        state.ResetCursor();
    }

    public void HandleInput()
    {
        ChangeState(state.HandleInput());
    }

    private void ChangeMaxHermiteAttempts(GameState state)
    {
        if (state == GameState.Training)
            maxHermiteAttempts = int.MaxValue;
        else
            maxHermiteAttempts = 3;
    }

    public void SwitchHermite()
    {
        ChangeState(state.SwitchHermite(state));
    }

    public void OnBezierDisabled(BezierType type)
    {
        ActiveCurves.Remove(type);
        NodeListDictionary[type].Clear();
        ChangeState(state.OnBezierDisabled(type));
    }

    public void OnFailPlacingNode()
    {
        SoundManager.PlaySound(failNodeSound);
    }

    private void ChangeState(BezierState newState)
    {
        if (newState != null)
        {
            state.Exit();
            state = newState;
            state.Enter(this, GetComponent<CursorComponent>());
        }
    }

    private void OnEnable()
    {
        state.SetStateCursor();
    }

    private void OnDisable()
    {
        state.ResetCursor();
    }

    private void OnMouseEnter()
    {
        if (!enabled) return;
        enteredDefRadar.Invoke();
        ChangeState(state.OnDefRadarIn());
    }

    private void OnMouseExit()
    {
        if (!enabled) return;
        outOfDefRadar.Invoke();
        ChangeState(state.OnDefRadarExit());
    }
}
