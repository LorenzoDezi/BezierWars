using UnityEngine;

public class AttackDefenseBezierState : BezierState
{
    public override BezierState HandleInput()
    {
        if (Input.GetButtonDown(spawner.SplineSpawnAxis) && spawner.Splines > 0)
            return hermiteState;
        BezierType type;
        if (Input.GetButtonDown(spawner.DefenseBezierAxisName))
            type = BezierType.Defense;
        else if (Input.GetButtonDown(spawner.AttackBezierAxisName))
            type = BezierType.Attack;
        else
            return null;

        var nodePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        nodePosition.z = 0;
        return HandleClick(type, nodePosition);
    }

    public override BezierState OnDefRadarExit()
    {
        return atkBezState;
    }

    /// <summary>
    /// Handles all the bezier logic for the user click.
    /// </summary>
    /// <param name="type">The bezier curve type</param>
    /// <param name="clickPosition"></param>
    /// <param name="nodeList"></param>
    protected BezierState HandleClick(
        BezierType type,
        Vector3 clickPosition
        )
    {
        if (RemoveBezierNode(clickPosition, type)) return null;
        var nodeList = spawner.NodeListDictionary[type];
        if (nodeList.Count == 2 && type == BezierType.Defense
            && CheckBezierCrossingSpawner(new Vector3[] {
                nodeList[0].transform.position, nodeList[1].transform.position, clickPosition
            }))
        {
            spawner.OnFailPlacingNode();
            return null;
        }
        var node = spawner.NodePrefabs[type];
        var instance = UnityEngine.GameObject.Instantiate(node);
        instance.transform.position = clickPosition;
        instance.GetComponent<FollowTargetComponent>()?.SetTargetToFollow(spawner.transform);
        nodeList.Add(instance);
        if (nodeList.Count == 3)
        {
            BuildBezier(type);
            if (type == BezierType.Attack) return BezierState.rmAtkDefBezState;
            else return BezierState.atkRmDefBezState;
        }
        return null;
    }

    
    

    
}

