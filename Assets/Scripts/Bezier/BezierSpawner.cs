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
    private string mouseXAxisName = "Mouse X";
    [SerializeField]
    private string mouseYAxisName = "Mouse Y";
    [SerializeField]
    private string defenseBezierAxisName = "DefenseBezierSpawn";
    [SerializeField]
    private string attackBezierAxisName = "AttackBezierSpawn";

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
    private float defenseBezierMaxDistance;
    [SerializeField]
    private float defenseBezierMinDistance = 2f;
    [SerializeField]
    private float removalNodeSenseDistance = 1f;

    [Header("Sound effects")]
    [SerializeField]
    private AudioClip bezierCreatedSound;

    private List<UnityEngine.GameObject> defenseActiveNodes = new List<UnityEngine.GameObject>();
    private List<UnityEngine.GameObject> attackActiveNodes = new List<UnityEngine.GameObject>();

    public BezierCreatedEvent OnBezierCreated { get; private set; }
    public UnityEvent OnFailNodePlacing { get; private set; }
    public UnityEvent Disabled { get; private set; }

    private void Awake()
    {
        OnBezierCreated = new BezierCreatedEvent();
        OnFailNodePlacing = new UnityEvent();
        Disabled = new UnityEvent();
    }

    public void PlaceDefenseNode()
    {
        var nodePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        nodePosition.z = 0;
        var distanceFromNode = Vector3.Distance(
            transform.position, nodePosition);
        if (distanceFromNode > defenseBezierMaxDistance
            || distanceFromNode < defenseBezierMinDistance)
            return;
        HandleClick(defenseBezNodePrefab, BezierType.Defense, nodePosition, defenseActiveNodes, transform);
    }

    public void PlaceAttackNode()
    {
        var nodePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        nodePosition.z = 0;
        HandleClick(attackBezNodePrefab, BezierType.Attack, nodePosition, attackActiveNodes);
    }

    private void Update()
    {
        
    }

    private UnityAction GenerateOnBezierDisableCallback(BezierType type)
    {
        if (type == BezierType.Attack)
            return () => { this.attackActiveNodes.Clear(); };
        else
            return () => { this.defenseActiveNodes.Clear(); };
    }

    private void HandleClick(
        UnityEngine.GameObject node,
        BezierType type,
        Vector3 nodePosition, 
        List<UnityEngine.GameObject> list,
        Transform parent = null
        )
    {
        //TODO: Refactor
        //Preliminar check
        if (CheckForBezierRemoval(nodePosition, type) 
            || CheckForNodeRemoval(nodePosition, list) || list.Count >= 3)
        {
            OnFailNodePlacing.Invoke();
            return;
        }
        if (list.Count == 2 && type == BezierType.Defense
            && CheckForBezierOnPlayer( new Vector3[] {
                list[0].transform.position, list[1].transform.position, nodePosition
            }))
        {
            OnFailNodePlacing.Invoke();
            return;
        }
        //Proceed to instantiate the node
        var instance = UnityEngine.GameObject.Instantiate(node);
        instance.transform.position = nodePosition;
        instance.GetComponent<FollowTargetComponent>()?.SetTargetToFollow(transform);
        list.Add(instance);
        //Add listener on bezSpawner disable (handling game state change)
        Disabled.AddListener(() => {
            list.Remove(instance);
            GameObject.Destroy(instance);
        });
        if (list.Count == 3)
        {
            BuildBezier(type, list);
            SoundManager.PlaySound(bezierCreatedSound, 1f);
        }
    }

    private void OnDisable()
    {
        Disabled.Invoke();
        Awake();
    }

    private bool CheckForBezierOnPlayer(Vector3[] nodes)
    {
        //TODO: set bezierLenght on spawner, and then pass to init 
        var bezierLength = 25f;
        for (int currIndex = 0; currIndex <= bezierLength; currIndex++)
        {
            Vector3 currentCurvePoint = BezierMath.Bernstein(
                currIndex / (float)bezierLength, 2, nodes);
            if (Vector3.Distance(currentCurvePoint, transform.position) < defenseBezierMinDistance)
                return true;
        }
        return false;
    }

    private bool CheckForBezierRemoval(Vector3 nodePosition, BezierType type)
    {
        var hits = Physics2D.CircleCastAll(nodePosition, 25f, Vector2.up, 0f);
        foreach(var hit in hits) {
            if (hit.collider != null)
            {
                var bezBuild = hit.collider.GetComponent<BezierBuilderComponent>();
                if (bezBuild != null && bezBuild.Type == type)
                {
                    hit.collider.GetComponent<HealthComponent>()?.ForceDeath();
                    return true;
                }
            }
        }
        return false;
    }

    private void BuildBezier(BezierType type, List<UnityEngine.GameObject> list)
    {
        UnityEngine.GameObject bezBuilder = BezierType.Attack == type ? attackBezBuilderPrefab : defenseBezBuilderPrefab;
        bezBuilder = UnityEngine.GameObject.Instantiate(bezBuilder);
        OnBezierCreated.Invoke(bezBuilder, type);
        var builderComp = bezBuilder.GetComponent<BezierBuilderComponent>();
        //Add listener on bezSpawner disable (handling game state change)
        Disabled.AddListener(() => GameObject.Destroy(bezBuilder));
        builderComp.Init(list, type);
        builderComp.Disabled.AddListener(GenerateOnBezierDisableCallback(type));
        if (BezierType.Defense == type)
        {
            bezBuilder.GetComponent<FollowTargetComponent>().SetTargetToFollow(transform);
            list.ForEach((obj) => UnityEngine.GameObject.Destroy(obj.GetComponent<FollowTargetComponent>()));
        }
    }

    private bool CheckForNodeRemoval(Vector3 nodePosition, List<UnityEngine.GameObject> list)
    {
        int activeNodeAtPosIndex = list.FindIndex(
                    (obj) => Vector3.Distance(obj.transform.position, nodePosition) <= removalNodeSenseDistance);
        if (activeNodeAtPosIndex != -1)
        {                
            var activeNode = list[activeNodeAtPosIndex];
            list.RemoveAt(activeNodeAtPosIndex);
            activeNode.GetComponent<IDamageable>().Die();
            return true;
        }
        return false;
    }
}
