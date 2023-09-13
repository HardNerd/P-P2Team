using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class defaultEnemy : EnemyAI
{
    [SerializeField] Transform shootPos;

    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;

    //bool isShooting;

    void Start()
    {
        startingPos = transform.position;
        stoppingDistanceOrig = agent.stoppingDistance;
        GameManager.instance.updatGameGoal(1);
    }

    void Update()
    {
        MoveEnemy();
    }

    //IEnumerator shoot()
    //{
    //    isShooting = true;
    //    Instantiate(bullet, shootPos.position, transform.rotation);
    //    yield return new WaitForSeconds(shootRate);
    //    isShooting = false;
    //}
}
