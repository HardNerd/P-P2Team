using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class meleeEnemyAI : EnemyAI
{
    [Header("----- Attack Stats -----")]
    [SerializeField] float attackRate;
    [SerializeField] int attackDamage;
    [SerializeField] int attackAngle;

    bool isAttacking;

    public int AttackDamage { get => attackDamage; }

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
            
            if (!isAttacking && angleToPlayer <= attackAngle)
                StartCoroutine(attack());
        }
    }

    IEnumerator attack()
    {
        isAttacking = true;

        RaycastHit hitInfo;

        if (Physics.Raycast(headPos.position, playerDirection, out hitInfo, agent.stoppingDistance))
        {
            IDamage damageable = hitInfo.collider.GetComponent<IDamage>();

            if (damageable != null)
                animator.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }
}
