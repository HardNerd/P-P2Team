using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class meleeEnemyAI : EnemyAI
{
    [SerializeField] float attackRate;
    [SerializeField] int attackDamage;

    bool isAttacking;

    void Start()
    {
        GameManager.instance.updatGameGoal(1);
    }

    void Update()
    {
        if (playerInRange)
        {
            playerDirection = GameManager.instance.player.transform.position - transform.position;

            if (agent.remainingDistance <= agent.stoppingDistance)
            {

                if (!isAttacking)
                    StartCoroutine(attack());
            }

        }
    }

    IEnumerator attack()
    {
        isAttacking = true;
        
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, playerDirection, out hitInfo, agent.stoppingDistance))
        {
            IDamage damageable = hitInfo.collider.GetComponent<IDamage>();

            if (damageable != null)
                damageable.TakeDamage(attackDamage);
        }

        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }
}
