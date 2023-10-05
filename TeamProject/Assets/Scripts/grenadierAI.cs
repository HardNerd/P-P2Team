using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class grenadierAI : EnemyAI
{

    [Header("----- Grenade Stats -----")]
    [SerializeField] GameObject coverPosition;
    [SerializeField] Transform throwPos;
    [SerializeField] GameObject molotov;
    [SerializeField] protected int attackDistance;

    protected bool isThrowing;
    protected bool inCover;
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
            if (!inCover)
            {
                StartCoroutine(TakeCover());
            }
            else
            {
                MoveEnemy(); // move towards player

                if (agent.remainingDistance <= agent.stoppingDistance && !isThrowing && playerInRange)
                {
                    StartCoroutine(ThrowMolotov());
                }
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
            inCover = true;
        }
    }

    IEnumerator ThrowMolotov()
    {
        isThrowing = true;
        Instantiate(molotov, throwPos.position, transform.rotation);
        yield return new WaitForSeconds(0.5f); // wait in front of player without attacking before going to cover
        inCover = false;
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
