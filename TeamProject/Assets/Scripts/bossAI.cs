using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bossAI : EnemyAI
{
    [SerializeField] Transform[] coverPositions;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject laserSight;
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
        playerDirection = GameManager.instance.transform.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, playerDirection.y, playerDirection.z), playerDirection);
        RaycastHit hit;
        Physics.Raycast(shootPos.transform.position, playerDirection, out hit);
        Debug.Log(angleToPlayer);
        laserSight.SetActive(true);
        
        yield return new WaitForSeconds(shootRate);
        laserSight.SetActive(false);
        Instantiate(laserSight, shootPos.transform.position, GameManager.instance.player.transform.rotation);
        Instantiate(bullet, shootPos.position, transform.rotation);
    }
}
