using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class defaultEnemy : EnemyAI
{
    [Header("----- Gun Stats -----")]
    [SerializeField] Transform shootPos;
    [SerializeField] float shootRate;
    [SerializeField] int shootAngle;
    [SerializeField] GameObject bullet;

    bool isShooting;

    void Start()
    {
        startingPos = transform.position;
        speedOrig = agent.speed;
        GameManager.instance.updatGameGoal(1);
    }

    void Update()
    {
        if (!isDead)
        {
            float agentVelocity = agent.velocity.normalized.magnitude;
            animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), agentVelocity, Time.deltaTime * animChangeSpeed));

            MoveEnemy();

            if (!isShooting && angleToPlayer <= shootAngle)
                StartCoroutine(shoot());
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        animator.SetTrigger("Shoot");
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
