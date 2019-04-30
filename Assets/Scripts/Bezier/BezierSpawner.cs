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
    [SerializeField]
    private string splineSpawnAxis = "SplineSpawn";

    [Header("Prefabs")]
    [SerializeField]
    private GameObject defenseNodePrefab;
    [SerializeField]
    private GameObject atkNodePrefab;
    [SerializeField]
    private GameObject defBuilderPrefab;
    [SerializeField]
    private GameObject atkBuilderPrefab;

    [Header("Parameters")]
    [SerializeField]
    private int bezierLength = 25;
    [SerializeField]
    private float defenseBezierMinDistance = 2f;
    [SerializeField]
    private float removalNodeSenseDistance = 2f;
    [SerializeField]
    private int maxSplines = 3;
    private int splines;

    [Header("Sound effects")]
    [SerializeField]
    private AudioClip bezierCreatedSound;
    private BezierState state;
    private UnityEvent enteredDefRadar;
    private UnityEvent outOfDefRadar;

    public UnityEvent OnFailNodePlacing { get; private set; }
    public string AttackBezierAxisName { get => attackBezierAxisName; }
    public string DefenseBezierAxisName { get => defenseBezierAxisName; }
    public float RemovalNodeSenseDistance { get => removalNodeSenseDistance; }
    
    public Dictionary<BezierType, GameObject> NodePrefabs { get; private set; }
    public Dictionary<BezierType, GameObject> BuilderPrefabs { get; private set; }
    public Dictionary<BezierType, List<GameObject>> NodeListDictionary { get; private set; }
    public string SplineSpawnAxis { get => splineSpawnAxis; }
    public int Splines { get => splines; }
    public int MaxSplines { get => maxSplines; }
    public AudioClip BezierCreatedSound { get => bezierCreatedSound; }
    public int BezierLength { get => bezierLength; }
    public Dictionary<BezierType, GameObject> ActiveCurves { get; private set; }
    public float DefenseBezierMinDistance { get => defenseBezierMinDistance; }

    private void Awake()
    {
        OnFailNodePlacing = new UnityEvent();
        enteredDefRadar = new UnityEvent();
        outOfDefRadar = new UnityEvent();
        enteredDefRadar.AddListener(CursorController.GetInstance().HandleInDefRadar);
        outOfDefRadar.AddListener(CursorController.GetInstance().HandleOutOfDefRadar);
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
        splines = maxSplines;
        state = BezierState.GetInitalState();
        state.Enter(this);
    }

    public void HandleInput()
    {
        ChangeState(state.HandleInput());
    }

    public void OnBezierDisabled(BezierType type)
    {
        ActiveCurves.Remove(type);
        NodeListDictionary[type].Clear();
        ChangeState(state.OnBezierDisabled(type));
    }

    private void ChangeState(BezierState newState)
    {
        if (newState != null)
        {
            state.Exit();
            state = newState;
            state.Enter(this);
        }
    }

    private void OnMouseEnter()
    {
        enteredDefRadar.Invoke();
        ChangeState(state.OnDefRadarIn());
    }

    private void OnMouseExit()
    {
        outOfDefRadar.Invoke();
        ChangeState(state.OnDefRadarExit());
    }
}
