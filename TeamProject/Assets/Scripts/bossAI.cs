using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bossAI : EnemyAI, IDamage
{
    [SerializeField] Transform[] coverPositions;
    [SerializeField] Transform shootPos;
    [SerializeField] Ray laser;
    [SerializeField] float shootRate;
    [SerializeField] int timeInCover;

    [SerializeField] GameObject bullet;

    public static bool inCover = false;

    void Start()
    {
        agent.SetDestination(coverPositions[0].transform.position);
    }
    void Update()
    {
        StartCoroutine(TakeCover());
        if (inCover)
        {
            StartCoroutine(shoot());
        }
    }

    IEnumerator TakeCover()
    {
        int selectedCoverPosition = Random.Range(0, (coverPositions.Length));
        if(!inCover && agent.remainingDistance == 0)
        {
            inCover = true;
            yield return new WaitForSeconds(timeInCover);
            agent.SetDestination(coverPositions[selectedCoverPosition].transform.position);
            inCover = false;
            
        }
    }

    IEnumerator shoot()
    {
        //animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(shootRate);
        laser.origin = shootPos.transform.position;
        laser.direction = GameManager.instance.player.transform.position;
        Instantiate(bullet, shootPos.position, transform.rotation);
    }
}
