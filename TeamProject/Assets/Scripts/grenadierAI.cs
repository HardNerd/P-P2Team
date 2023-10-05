using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class grenadierAI : EnemyAI
{
    public enum GrenadierState
    {
        GoToCover,
        InCover,
        ChasePlayer,
        Attack
    }

    [Header("----- Grenade Stats -----")]
    [SerializeField] GameObject coverPosition;
    [SerializeField] Transform throwPos;
    [SerializeField] GameObject molotov;
    [SerializeField] protected int attackDistance;
    [SerializeField] int maxThrows;
    [SerializeField] float timeBetweenThrows;

    [SerializeField] GrenadierState _currentState = GrenadierState.ChasePlayer;

    protected bool isThrowing;
    protected bool inCover;
    float agentStoppingDistOrig;
    int molotovsThrown = 0;

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
            switch (_currentState)
            {
                case GrenadierState.GoToCover:
                    GoToCover();
                    break;
                case GrenadierState.InCover:
                    StartCoroutine(TakeCover());
                    break;
                case GrenadierState.ChasePlayer:
                    MoveEnemy();
                    break;
                case GrenadierState.Attack:
                    Attack();
                    break;
                default:
                    break;
            }
        }
    }

    void SwitchToNextState(GrenadierState nextState)
    {
        _currentState = nextState;
    }

    void GoToCover()
    {
        agent.stoppingDistance = 0;
        agent.SetDestination(coverPosition.transform.position);

        if (agent.remainingDistance <= agent.stoppingDistance)
            SwitchToNextState(GrenadierState.InCover);
    }

    IEnumerator TakeCover()
    {
        if (!inCover)
        {
            inCover = true;
            molotovsThrown = 0;
            yield return new WaitForSeconds(3);
            agent.stoppingDistance = agentStoppingDistOrig;
            inCover = false;
            SwitchToNextState(GrenadierState.ChasePlayer);
        }
    }

    protected override void MoveEnemy()
    {
        base.MoveEnemy();

        if (agent.remainingDistance <= agent.stoppingDistance)
            SwitchToNextState(GrenadierState.Attack);
    }

    void Attack()
    {
        if (molotovsThrown >= maxThrows)
        {
            SwitchToNextState(GrenadierState.GoToCover);
            return;
        }

        // if player moves away
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            SwitchToNextState(GrenadierState.ChasePlayer);
            return;
        }

        if (!isThrowing)
            StartCoroutine(ThrowMolotov());
    }

    IEnumerator ThrowMolotov()
    {
        isThrowing = true;
        molotovsThrown++;
        Instantiate(molotov, throwPos.position, transform.rotation);
        yield return new WaitForSeconds(timeBetweenThrows); // wait in front of player without attacking before going to cover
        isThrowing = false;
    }
}
