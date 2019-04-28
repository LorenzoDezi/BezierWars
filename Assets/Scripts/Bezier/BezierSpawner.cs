using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BezierCreatedEvent : UnityEvent<GameObject, BezierType> { }
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
    private UnityEngine.GameObject defenseBezNodePrefab;
    [SerializeField]
    private UnityEngine.GameObject attackBezNodePrefab;
    [SerializeField]
    private UnityEngine.GameObject attackBezBuilderPrefab;
    [SerializeField]
    private UnityEngine.GameObject defenseBezBuilderPrefab;

    [Header("Parameters")]
    [SerializeField]
    private int bezierLength = 25;
    [SerializeField]
    private float defenseBezierMaxDistance;
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

    private List<UnityEngine.GameObject> defenseActiveNodes = new List<UnityEngine.GameObject>();
    private List<UnityEngine.GameObject> attackActiveNodes = new List<UnityEngine.GameObject>();
    private IBezierState state;

    public BezierCreatedEvent OnBezierCreated { get; private set; }
    public UnityEvent OnFailNodePlacing { get; private set; }
    public string AttackBezierAxisName { get => attackBezierAxisName; }
    public string DefenseBezierAxisName { get => defenseBezierAxisName; }
    public float RemovalNodeSenseDistance { get => removalNodeSenseDistance; }
    public float DefenseBezierMaxDistance { get => defenseBezierMaxDistance; }
    public float DefenseBezierMinDistance { get => defenseBezierMinDistance; }
    public GameObject DefenseBezNodePrefab { get => defenseBezNodePrefab; }
    public GameObject AttackBezNodePrefab { get => attackBezNodePrefab; }
    public GameObject AttackBezBuilderPrefab { get => attackBezBuilderPrefab; }
    public GameObject DefenseBezBuilderPrefab { get => defenseBezBuilderPrefab; }
    public List<GameObject> DefenseActiveNodes { get => defenseActiveNodes; }
    public List<GameObject> AttackActiveNodes { get => attackActiveNodes; }
    public string SplineSpawnAxis { get => splineSpawnAxis; }
    public int Splines { get => splines; }
    public int MaxSplines { get => maxSplines; }
    public AudioClip BezierCreatedSound { get => bezierCreatedSound; }
    public int BezierLength { get => bezierLength; }

    private void Awake()
    {
        OnBezierCreated = new BezierCreatedEvent();
        OnFailNodePlacing = new UnityEvent();
        splines = maxSplines;
        state = new BezierState();
        state.Enter(this);
    }

    public void HandleInput()
    {
        var newState = state.HandleInput();
        if(newState != null)
        {
            state.Exit();
            state = newState;
            state.Enter(this);
        }
    }
}
