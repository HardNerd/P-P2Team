using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
  public Transform WaypointGet(int wayPointNumber)
    {
        return transform.GetChild(wayPointNumber);
    }

    public int NextWaypoint(int currWayPoint)
    {
        int nextWayPoint = currWayPoint + 1;

        if (nextWayPoint == transform.childCount )
        {
            nextWayPoint = 0;
        }
        return nextWayPoint;
    }
}
