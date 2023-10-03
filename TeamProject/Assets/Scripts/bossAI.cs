using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bossAI : EnemyAI, IDamage
{
    [SerializeField] Transform[] coverPositions;
    [SerializeField] int timeInCover;

    bool inCover = false;

    void Update()
    {
        StartCoroutine(TakeCover());
    }

    IEnumerator TakeCover()
    {
        if(!inCover)
        {
            inCover = true;
            agent.SetDestination(coverPositions[Random.Range(0, (coverPositions.Length - 1))].transform.position);
            yield return new WaitForSeconds(timeInCover);
            inCover = false;
        }
    }
}
