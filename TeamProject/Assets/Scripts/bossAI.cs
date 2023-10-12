using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static grenadierAI;

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
    [SerializeField] Transform shootPos;
    [SerializeField] int attackCountdown;

    [SerializeField] GameObject bullet;

    [SerializeField] SniperState _currentState;

    [Header("----- BOSS Power Up -----")]
    [SerializeField] Transform dropLocation;

    LineRenderer laserSight;
    public static bool takingCover;
    private bool isAiming;
    int selectedCoverPosition;

    void Start()
    {
        laserSight = GetComponent<LineRenderer>();
        B_footR = dropLocation;
        agent.SetDestination(coverPositions[0].transform.position);
        laserSight.enabled = false;
    }
    void Update()
    {
        if (!isDead)
        {
            switch (_currentState)
            {
                case SniperState.SelectCover:
                    selectedCoverPosition = Random.Range(0, coverPositions.Length);
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
            agent.SetDestination(coverPositions[selectedCoverPosition].transform.position);
            return;
        }

        if (agent.remainingDistance <= 0)
        {
            takingCover = false;
            SwitchToState(SniperState.Aim);
        }
    }

    IEnumerator Aim()
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

    void ActivateLaser()
    {
        laserSight.enabled = true;
        laserSight.SetPosition(0, shootPos.transform.position);
        RaycastHit hit;
        Debug.DrawRay(headPos.position, playerDirection);

        if (Physics.Raycast(shootPos.transform.position, playerDirection + new Vector3(0, -3f, 0), out hit))
            laserSight.SetPosition(1, hit.point);
    }

    private void Shoot()
    {
        Instantiate(bullet, shootPos.transform.position, Quaternion.LookRotation(playerDirection + new Vector3(0, -3f, 0)));
        laserSight.enabled = false;

        SwitchToState(SniperState.SelectCover);
    }

    public override void TakeDamage(float amount, string source)
    {
        base.TakeDamage(amount, source);

        if (HP <= 0)
        {
            for (int i = 0; i < roomDoors.Length; i++)
                roomDoors[i].SetActive(false);
        }
    }
}
