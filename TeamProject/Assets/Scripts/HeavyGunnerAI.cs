using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeavyGunnerAI : EnemyAI
{
    [SerializeField] Transform shootPos;

    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;

    bool isShooting;

    void Start()
    {
        GameManager.instance.updatGameGoal(1);
    }

    void Update()
    {
        if (playerInRange && CanSeePlayer())
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!isShooting)
                    StartCoroutine(shoot());
            }
        }

    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

}
