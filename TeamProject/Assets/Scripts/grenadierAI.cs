using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class grenadierAI : EnemyAI
{
    [SerializeField] GameObject coverPosition;

    [Header("----- Grenade Stats -----")]
    [SerializeField] Transform throwPos;
    [SerializeField] GameObject molotov;
    [SerializeField] int attackDistance;

    bool isThrowing;
    bool isTakingCover;
    float agentStoppingDistOrig;
    bool playerInRange = false;

    void Start()
    {
        agent.stoppingDistance = attackDistance;
        agentStoppingDistOrig = agent.stoppingDistance;
        speedOrig = agent.speed;
    }

    void Update()
    {
        if (!isDead)
        {
            GetComponent<SphereCollider>().radius = agent.stoppingDistance;
            if (!isTakingCover)
            {
                MoveEnemy();

                if (agent.remainingDistance <= agent.stoppingDistance && !isTakingCover && !isThrowing && playerInRange)
                {
                    StartCoroutine(ThrowMolotov());
                    isTakingCover = true;
                }
            }
            else
            {
                StartCoroutine(TakeCover());
            }
            float agentVelocity = agent.velocity.normalized.magnitude;
            animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), agentVelocity, Time.deltaTime * animChangeSpeed));
        }
    }

    IEnumerator TakeCover()
    {
        agent.stoppingDistance = 0;
        agent.SetDestination(coverPosition.transform.position);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            yield return new WaitForSeconds(3);
            agent.stoppingDistance = agentStoppingDistOrig;
            isTakingCover = false;
        }
    }

    IEnumerator ThrowMolotov()
    {
        isThrowing = true;
        Instantiate(molotov, throwPos.position, transform.rotation);
        yield return new WaitForSeconds(3);
        isThrowing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
