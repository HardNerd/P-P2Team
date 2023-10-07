using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class defaultEnemy : EnemyAI
{
    [Header("----- Gun Stats -----")]
    [SerializeField] protected Transform shootPos;
    [SerializeField] protected float shootRate;
    [SerializeField] int shootAngle;
    [SerializeField] protected GameObject bullet;

    protected bool isShooting;

    void Start()
    {
        speedOrig = agent.speed;
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

    virtual protected IEnumerator shoot()
    {
        isShooting = true;
        animator.SetTrigger("Shoot");
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
