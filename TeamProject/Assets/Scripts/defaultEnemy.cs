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
    [SerializeField] protected int shootAngle;
    [SerializeField] protected GameObject bullet;

    protected bool isShooting;
    bool canShoot = true;

    private void Awake()
    {
        healthBar = GetComponentInChildren<enemyHealthBar>();
    }

    void Start()
    {
        speedOrig = agent.speed;
        maxHP = HP;
        healthBar.UpdateHealthBar(HP, maxHP);
        healthObj.SetActive(false);
    }

    void Update()
    {
        if (!isDead)
        {
            float agentVelocity = agent.velocity.normalized.magnitude;
            animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), agentVelocity, Time.deltaTime * animChangeSpeed));

            MoveEnemy();

            if (!isShooting && angleToPlayer <= shootAngle && canShoot)
                StartCoroutine(shoot());
        }
    }

    virtual protected IEnumerator shoot()
    {
        isShooting = true;
        animator.SetTrigger("Shoot");
        Instantiate(bullet, shootPos.position, Quaternion.LookRotation(playerDirection));
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    protected override IEnumerator StopMoving()
    {
        agent.speed = 0;
        canShoot = false;
        yield return new WaitForSeconds(stopAtDamageTime);
        canShoot = true;
        agent.speed = speedOrig;
    }
}
