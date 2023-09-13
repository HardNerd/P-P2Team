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

    void Start()
    {
        startingPos = transform.position;
        stoppingDistanceOrig = agent.stoppingDistance;
        GameManager.instance.updatGameGoal(1);
    }

    void Update()
    {
        MoveEnemy();

        if (playerInSight && !isAttacking && angleToPlayer <= attackAngle)
            StartCoroutine(attack());
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
