using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class sniper : bossAI
{

    private void Awake()
    {
        healthBar = GetComponentInChildren<enemyHealthBar>();
    }

    void Start()
    {
        laserSight = GetComponent<LineRenderer>();
        laserSight.enabled = false;
        maxHP = HP;
        healthBar.UpdateHealthBar(HP, maxHP);
        healthObj.SetActive(true);
    }

    void Update()
    {
        StartCoroutine(Aim());
    }

    protected override IEnumerator Aim()
    {
        //turn towards player
        playerDirection = GameManager.instance.player.transform.position - transform.position;
        FaceTarget(playerDirection);

        ActivateLaser();

        if (!isAiming)
        {
            isAiming = true;
            yield return new WaitForSeconds(attackCountdown);
            Shoot();
            isAiming = false;
        }
    }

    protected override void Shoot()
    {
        animator.SetTrigger("Shoot");
        Instantiate(bullet, shootPos.transform.position, Quaternion.LookRotation(playerDirection + new Vector3(0, -3f, 0)));
        laserSight.enabled = false;
    }
}
