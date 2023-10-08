using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class superGrenadier : grenadierAI
{
    [SerializeField] GameObject[] attackPositions;

    [Header("----- BOSS Power Up -----")]
    [SerializeField] Transform dropLocation;

    Vector3 closestAttackPos;
    int randomMaxThrows;
    bool hasSetRandomThrows;

    void Start()
    {
        B_footR = dropLocation;
        _currentState = GrenadierState.GoToCover;
        agent.stoppingDistance = attackDistance;
        agentStoppingDistOrig = agent.stoppingDistance;
        speedOrig = agent.speed;
    }

    void Update()
    {
        if (!isDead)
        {
            switch (_currentState)
            {
                case GrenadierState.GoToCover:
                    GoToCover();
                    break;
                case GrenadierState.InCover:
                    StartCoroutine(TakeCover());
                    break;
                case GrenadierState.DetermineAttackPos:
                    closestAttackPos = DetermineClosestPos();
                    break;
                case GrenadierState.ChasePlayer:
                    MoveToClosestPos();
                    break;
                case GrenadierState.Attack:
                    Attack();
                    break;
                default:
                    break;
            }
        }
    }

    protected override IEnumerator TakeCover()
    {
        if (!inCover)
        {
            inCover = true;
            molotovsThrown = 0;
            yield return new WaitForSeconds(3);
            agent.stoppingDistance = agentStoppingDistOrig;
            inCover = false;
            SwitchToNextState(GrenadierState.DetermineAttackPos);
        }
    }

    Vector3 DetermineClosestPos()
    {
        GameObject closestPos = attackPositions[0];
        for (int i = 1; i < attackPositions.Length; i++)
        {
            if (Vector3.Distance(attackPositions[i].transform.position, GameManager.instance.player.transform.position) < Vector3.Distance(closestPos.transform.position, GameManager.instance.player.transform.position))
                closestPos = attackPositions[i];
        }
        SwitchToNextState(GrenadierState.ChasePlayer);
        return closestPos.transform.position;
    }

    void MoveToClosestPos()
    {
        agent.SetDestination(closestAttackPos);

        if (agent.remainingDistance <= agent.stoppingDistance)
            SwitchToNextState(GrenadierState.Attack);
    }

    protected override void Attack()
    {
        // Sets throws to vary between the max throws set and -1 that amount
        if (!hasSetRandomThrows)
        {
            randomMaxThrows = Random.Range(maxThrows, maxThrows - 1);
            hasSetRandomThrows = true;
        }

        if (molotovsThrown >= randomMaxThrows)
        {
            hasSetRandomThrows = false;
            SwitchToNextState(GrenadierState.GoToCover);
            return;
        }

        if (!isThrowing)
            StartCoroutine(ThrowMolotov());
    }
}
