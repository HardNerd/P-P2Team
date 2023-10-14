using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class bossAI : EnemyAI
{
    public enum SniperState
    {
        SelectCover,
        GoToCover,
        Aim,
        Attack,
    }

    [SerializeField] GameObject[] roomDoors;
    [SerializeField] Transform[] coverPositions;
    [SerializeField] protected Transform shootPos;
    [SerializeField] protected int attackCountdown;

    [SerializeField] protected GameObject bullet;

    [SerializeField] SniperState _currentState;

    [Header("----- BOSS Power Up -----")]
    [SerializeField] Transform dropLocation;

    protected LineRenderer laserSight;
    public static bool takingCover;
    protected bool isAiming;
    int currentCoverPosition = 0;
    int selectedCoverPosition = 0;

    private void Awake()
    {
        healthBar = GetComponentInChildren<enemyHealthBar>();
    }

    void Start()
    {
        laserSight = GetComponent<LineRenderer>();
        B_footR = dropLocation;
        agent.SetDestination(coverPositions[0].transform.position);
        laserSight.enabled = false;
        maxHP = HP;
        healthBar.UpdateHealthBar(HP, maxHP);
        healthObj.SetActive(true);
    }
    void Update()
    {
        if (!isDead)
        {
            switch (_currentState)
            {
                case SniperState.SelectCover:
                    while (currentCoverPosition == selectedCoverPosition)
                        selectedCoverPosition = Random.Range(0, coverPositions.Length);
                    currentCoverPosition = selectedCoverPosition;
                    SwitchToState(SniperState.GoToCover);
                    break;
                case SniperState.GoToCover:
                    TakeCover();
                    break;
                case SniperState.Aim:
                    StartCoroutine(Aim());
                    break;
                case SniperState.Attack:
                    Shoot();
                    break;
                default:
                    break;
            }
            float agentVelocity = agent.velocity.normalized.magnitude;
            animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), agentVelocity, Time.deltaTime * animChangeSpeed));
        }
    }

    protected void SwitchToState(SniperState nextState)
    {
        _currentState = nextState;
    }

    void TakeCover()
    {
        if (!takingCover)
        {
            takingCover = true;
            agent.SetDestination(coverPositions[currentCoverPosition].transform.position);
            return;
        }

        if (agent.remainingDistance <= 0)
        {
            takingCover = false;
            isInvincible = true;
            SwitchToState(SniperState.Aim);
        }
    }

    virtual protected IEnumerator Aim()
    {
        //turn towards player
        playerDirection = GameManager.instance.player.transform.position - transform.position;
        FaceTarget(playerDirection);

        ActivateLaser();

        if (!isAiming)
        {
            isAiming = true;
            yield return new WaitForSeconds(attackCountdown);
            SwitchToState(SniperState.Attack);
            isAiming = false;
        }
    }

    protected void ActivateLaser()
    {
        laserSight.enabled = true;
        laserSight.SetPosition(0, shootPos.transform.position);
        RaycastHit hit;
        Debug.DrawRay(headPos.position, playerDirection);

        if (Physics.Raycast(shootPos.transform.position, playerDirection + new Vector3(0, -3f, 0), out hit))
            laserSight.SetPosition(1, hit.point);
    }

    virtual protected void Shoot()
    {
        animator.SetTrigger("Shoot");
        Instantiate(bullet, shootPos.transform.position, Quaternion.LookRotation(playerDirection + new Vector3(0, -3f, 0)));
        laserSight.enabled = false;
        isInvincible = false;

        SwitchToState(SniperState.SelectCover);
    }

    public override void TakeDamage(float amount, string source)
    {
        base.TakeDamage(amount, source);

        if (HP <= 0)
        {
            for (int i = 0; i < roomDoors.Length; i++)
                roomDoors[i].SetActive(false);
            laserSight.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spawner"))
            coverPositions = other.GetComponent<spawner>().coverPositions;
    }
}
