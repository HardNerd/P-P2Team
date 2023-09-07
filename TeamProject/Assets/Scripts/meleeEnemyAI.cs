using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class meleeEnemyAI : MonoBehaviour
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Range(1, 5)][SerializeField] int HP;
    [SerializeField] int targetFaceSpeed;

    [SerializeField] float attackRate;
    [SerializeField] int attackDamage;

    Vector3 playerDirection;
    bool playerInRange;
    bool isAttacking;

    void Start()
    {

    }

    void Update()
    {
        if (playerInRange)
        {
            playerDirection = GameManager.instance.player.transform.position - transform.position;

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                faceTarget();

                if (!isAttacking)
                    StartCoroutine(attack());
            }

            agent.SetDestination(GameManager.instance.player.transform.position);
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
                damageable.takeDamage(attackDamage);
        }

        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }

    public void takeDamage(int damageAmount)
    {
        HP -= damageAmount;
        StartCoroutine(hitFlash());

        if (HP <= 0)
        {
            Destroy(gameObject);
            GameManager.instance.updatGameGoal(-1);
        }
    }

    IEnumerator hitFlash()
    {
        Color origColor = model.material.color;

        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = origColor;
    }

    void faceTarget()
    {
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * targetFaceSpeed);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
