using UnityEngine;

public class RemoveAttackDefenseBezierState : BezierState
{
    public override BezierState HandleInput()
    {
        if (Input.GetButtonDown(spawner.SplineSpawnAxis) && spawner.Splines > 0)
            return hermiteState;
        if (Input.GetButtonDown(spawner.AttackBezierAxisName))
        {
            RemoveBezier(BezierType.Attack);
            return atkDefBezState;
        }
        else if (Input.GetButtonDown(spawner.DefenseBezierAxisName))
        {
            var nodePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            nodePosition.z = 0;
            return HandleClick(nodePosition);
        }
        else
            return null;
    }

    protected BezierState HandleClick(Vector3 clickPosition)
    {
        if (RemoveBezierNode(clickPosition, BezierType.Defense)) return null;
        var nodeList = spawner.NodeListDictionary[BezierType.Defense];
        if (nodeList.Count == 2 && CheckBezierCrossingSpawner(new Vector3[] {
                nodeList[0].transform.position, nodeList[1].transform.position, clickPosition
            }))
        {
            spawner.OnFailNodePlacing.Invoke();
            return null;
        }
        var node = spawner.NodePrefabs[BezierType.Defense];
        var instance = UnityEngine.GameObject.Instantiate(node);
        instance.transform.position = clickPosition;
        instance.GetComponent<FollowTargetComponent>().SetTargetToFollow(spawner.transform);
        nodeList.Add(instance);
        if (nodeList.Count == 3)
        {
            BuildBezier(BezierType.Defense);
            return BezierState.rmAtkRmDefBezState;
        }
        return null;
    }

    public override BezierState OnDefRadarExit()
    {
        return rmAtkBezState;
    }

    public override BezierState OnBezierDisabled(BezierType type)
    {
        if (type == BezierType.Attack)
            return atkDefBezState;
        return null;
    }
}