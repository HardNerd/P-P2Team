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
        Attack,
        DetermineAttackPos
    }

    [Header("----- Grenade Stats -----")]
    [SerializeField] Vector3 coverPosition;
    [SerializeField] Transform throwPos;
    [SerializeField] GameObject molotov;
    [SerializeField] protected int attackDistance;
    [SerializeField] protected int maxThrows;
    [SerializeField] float timeBetweenThrows;

    [SerializeField] protected GrenadierState _currentState = GrenadierState.ChasePlayer;

    protected bool isThrowing;
    protected bool inCover;
    protected float agentStoppingDistOrig;
    protected int molotovsThrown = 0;
    protected int animCount = 0;

    private void Awake()
    {
        healthBar = GetComponentInChildren<enemyHealthBar>();
    }

    void Start()
    {
        agent.stoppingDistance = attackDistance;
        agentStoppingDistOrig = agent.stoppingDistance;
        speedOrig = agent.speed;
        maxHP = HP;
        healthBar.UpdateHealthBar(HP, maxHP);
        healthObj.SetActive(false);
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
            float agentVelocity = agent.velocity.normalized.magnitude;
            animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), agentVelocity, Time.deltaTime * animChangeSpeed));
        }
    }

    protected void SwitchToNextState(GrenadierState nextState)
    {
        _currentState = nextState;
    }

    protected void GoToCover()
    {
        agent.stoppingDistance = 0;
        agent.SetDestination(coverPosition);

        if (agent.remainingDistance <= agent.stoppingDistance)
            SwitchToNextState(GrenadierState.InCover);
    }

    virtual protected IEnumerator TakeCover()
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

    virtual protected void Attack()
    {
        if (molotovsThrown >= maxThrows)
        {
            if (animCount >= maxThrows)
            {
                animCount = 0;
                SwitchToNextState(GrenadierState.GoToCover);
            }
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

    protected IEnumerator ThrowMolotov()
    {
        isThrowing = true;
        molotovsThrown++;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(timeBetweenThrows); // wait in front of player without attacking before going to cover
        isThrowing = false;
    }

    public void InstantiateMolotov()
    {
        AudioSource source = molotov.GetComponent<AudioSource>();
        float pitch = source.pitch;
        GameManager.instance.AudioChange(source);
        Instantiate(molotov, throwPos.position, transform.rotation);
        source.pitch = pitch;
    }

    public void AnimEnd()
    {
        animCount++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cover Pos"))
            coverPosition = other.GetComponent<Transform>().position;
    }
}
