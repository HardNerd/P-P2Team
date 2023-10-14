using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketMan : defaultEnemy
{
    [SerializeField] Transform shootPos2;
    [SerializeField] ParticleSystem gunExplosion;

    bool shootPosToggle;

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

            if (agentVelocity > 0.01f)
                animator.SetBool("Moving", true);
            else
                animator.SetBool("Moving", false);

            MoveEnemy();

            if (!isShooting && angleToPlayer <= shootAngle)
                StartCoroutine(shoot());
        }
    }

    protected override IEnumerator shoot()
    {
        isShooting = true;
        animator.SetTrigger("Shoot");

        if (shootPosToggle)
        {
            Instantiate(bullet, shootPos.position, Quaternion.LookRotation(playerDirection));
            Instantiate(gunExplosion, shootPos.position, Quaternion.LookRotation(playerDirection));
        }
        else
        {
            Instantiate(bullet, shootPos2.position, Quaternion.LookRotation(playerDirection));
            Instantiate(gunExplosion, shootPos2.position, Quaternion.LookRotation(playerDirection));
        }

        yield return new WaitForSeconds(shootRate);
        shootPosToggle = !shootPosToggle;
        isShooting = false;
    }
}
