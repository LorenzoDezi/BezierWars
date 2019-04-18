using BezierWars.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace BezierWars.Bezier
{
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
        private GameObject defenseBezNodePrefab;
        [SerializeField]
        private GameObject attackBezNodePrefab;
        [SerializeField]
        private GameObject attackBezBuilderPrefab;
        [SerializeField]
        private GameObject defenseBezBuilderPrefab;

        [Header("Parameters")]
        [SerializeField]
        private float defenseBezierMaxDistance;
        [SerializeField]
        private float removalNodeSenseDistance = 1f;

        private List<GameObject> defenseActiveNodes = new List<GameObject>();
        private List<GameObject> attackActiveNodes = new List<GameObject>();


        private void Update()
        {
            if (Input.GetButtonDown(defenseBezierAxisName))
            {
                var nodePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                nodePosition.z = 0;
                if (Vector3.Distance(transform.position, nodePosition) > defenseBezierMaxDistance)
                    return;
                HandleClick(defenseBezNodePrefab, BezierType.Defense, nodePosition, defenseActiveNodes, transform);

            }
            else if (Input.GetButtonDown(attackBezierAxisName))
            {
                var nodePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                nodePosition.z = 0;
                HandleClick(attackBezNodePrefab, BezierType.Attack, nodePosition, attackActiveNodes);
            }
        }

        private UnityAction GenerateOnBezierDisableCallback(BezierType type)
        {
            if (type == BezierType.Attack)
                return () => { this.attackActiveNodes.Clear(); };
            else
                return () => { this.defenseActiveNodes.Clear(); };
        }

        private void HandleClick(
            GameObject node, BezierType type,
            Vector3 nodePosition, List<GameObject> list,
            Transform parent = null
            )
        {
            //Preliminar check
            if (list.Count >= 3)
                return;
            //Remove check: if the node is near another already placed, the player intent is to delete it
            CheckForNodeRemoval(nodePosition, list);
            //Proceed to instantiate the node
            //TODO: Check max distance between each node: valutate if there is need for this
            PlaceNode(node, nodePosition, list, parent);
            if (list.Count == 3)
            {
                PlaceCurve(type, list);
            }
        }

        private void PlaceCurve(BezierType type, List<GameObject> list)
        {
            GameObject bezBuilder = BezierType.Attack == type ? attackBezBuilderPrefab : defenseBezBuilderPrefab;
            bezBuilder = GameObject.Instantiate(bezBuilder);
            var builderComp = bezBuilder.GetComponent<BezierBuilderComponent>();
            builderComp.Init(list);
            builderComp.Disabled.AddListener(GenerateOnBezierDisableCallback(type));
            if (BezierType.Defense == type)
                bezBuilder.GetComponent<DefenseBezierComponent>().SetTargetToFollow(transform);
        }

        private static void PlaceNode(GameObject node, Vector3 nodePosition,
            List<GameObject> list, Transform parent)
        {
            var instance = GameObject.Instantiate(node);
            instance.transform.parent = parent;
            instance.transform.position = nodePosition;
            list.Add(instance);
        }

        private void CheckForNodeRemoval(Vector3 nodePosition, List<GameObject> list)
        {
            int activeNodeAtPosIndex = list.FindIndex(
                        (obj) => Vector3.Distance(obj.transform.position, nodePosition) <= removalNodeSenseDistance);
            if (activeNodeAtPosIndex != -1)
            {
                var activeNode = list[activeNodeAtPosIndex];
                list.RemoveAt(activeNodeAtPosIndex);
                activeNode.GetComponent<IDamageable>().Die();
            }
        }
    }

}