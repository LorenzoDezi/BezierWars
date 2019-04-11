using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(EnemyController))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;


    private Seeker seeker;
    private EnemyController controller;

    private Path path;
    private int currentWaypoint = 0;
    [SerializeField]
    private float nextWaypointDistance = 3;
    [SerializeField]
    private float rePathRate = 0.5f;
    private float lastRepath = float.NegativeInfinity;
    private bool reachedEndOfPath;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        controller = GetComponent<EnemyController>();
    }

    private float GetWaypointDistance()
    {
        reachedEndOfPath = false;
        float distanceToWaypoint;
        //We check in a loop if it has reached the waypoint, because the ai can reach
        //several waypoints at once since they are close with each other.
        while (true)
        {
            //TODO: Check performance
            distanceToWaypoint = Vector2.Distance(
                    new Vector2(transform.position.x, transform.position.y),
                    new Vector2(path.vectorPath[currentWaypoint].x,
                        path.vectorPath[currentWaypoint].y)
                );
            if (distanceToWaypoint < nextWaypointDistance)
            {
                if (currentWaypoint + 1 < path.vectorPath.Count)
                    currentWaypoint++;
                else
                {
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
                break;
        }
        return distanceToWaypoint;
    }

    private void Update()
    {
        if(Time.time > lastRepath + rePathRate && seeker.IsDone())
        {
            lastRepath = Time.time;
            seeker.StartPath(transform.position, target.position + offset, OnPathComplete);
        }
        if (path == null)
            return;
        float waypointDistance = GetWaypointDistance();
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(waypointDistance / nextWaypointDistance) : 1f;
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        controller.Move(dir, speedFactor);
    }

    public void OnPathComplete(Path p)
    {
        p.Claim(this);
        if (!p.error)
        {
            if (path != null)
                path.Release(this);
            path = p;
            currentWaypoint = 0;
        }
        else
            p.Release(this);
    }
}
