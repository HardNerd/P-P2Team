using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private WayPoint wayPoint;
    [SerializeField] float speed;

    private int targetWayPointNum;

    private Transform previousWaypoint;
    private Transform targetWayPoint;

    private float timeToWayPoint;
    private float timeElapsed;

    private void Start()
    {
        TargetNextWayPoint();
    }

    private void FixedUpdate()
    {
        timeElapsed += Time.deltaTime;

        float percentageElapsed = timeElapsed / timeToWayPoint;
        percentageElapsed = Mathf.SmoothStep(0,1,percentageElapsed);
        transform.position = Vector3.Lerp(previousWaypoint.position, targetWayPoint.position, percentageElapsed);

        if(percentageElapsed >= 1)
        {
            TargetNextWayPoint();
        }

    }

    private void TargetNextWayPoint()
    {
        previousWaypoint = wayPoint.WaypointGet(targetWayPointNum);
        targetWayPointNum = wayPoint.NextWaypoint(targetWayPointNum);
        targetWayPoint = wayPoint.WaypointGet(targetWayPointNum);

        timeElapsed = 0;

        float disatanceFromWaypoint = Vector3.Distance(previousWaypoint.position, targetWayPoint.position);

        timeToWayPoint = disatanceFromWaypoint / speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }

}
