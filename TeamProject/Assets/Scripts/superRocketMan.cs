using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static grenadierAI;

public class superRocketMan : EnemyAI
{
    public enum SuperRMState
    {
        Idle,
        ChooseTargets,
        Attack,
        Wait,
        DetermineNextPos,
        ChangePos
    }

    [SerializeField] float flySpeed;
    [SerializeField] float waitTime;

    [Header("----- Shoot Stats -----")]
    [SerializeField] Transform shootPos;
    [SerializeField] float shootRate;
    [SerializeField] GameObject projectile;

    [SerializeField] GameObject[] attackPositions;

    [SerializeField] SuperRMState _currentState;

    Vector3 selectedPos;
    bool isWaiting;

    void Start()
    {
        enemyBody = GetComponent<Rigidbody>();
        selectedPos = attackPositions[0].transform.position;
    }

    void Update()
    {
        if (!isDead)
        {
            switch (_currentState)
            {
                case SuperRMState.Idle:
                    break; 
                case SuperRMState.ChooseTargets:
                    SelectTargets();
                    break;
                case SuperRMState.Attack:
                    Attack();
                    break;
                case SuperRMState.Wait:
                    StartCoroutine(WaitAfterAttack());
                    break;
                case SuperRMState.DetermineNextPos:
                    selectedPos = ChooseRandomPos();
                    break;
                case SuperRMState.ChangePos:
                    MoveToPosition();
                    break;
                default:
                    break;
            }
        }
        else
            enemyBody.useGravity = true;
    }

    void SelectTargets()
    {
        SwitchToNextState(SuperRMState.Attack);
    }

    void Attack()
    {
        SwitchToNextState(SuperRMState.Wait);
    }

    IEnumerator WaitAfterAttack()
    {
        if (!isWaiting)
        {
            isWaiting = true;
            yield return new WaitForSeconds(waitTime);
            SwitchToNextState(SuperRMState.DetermineNextPos);
            isWaiting = false;
        }
    }

    Vector3 ChooseRandomPos()
    {
        Vector3 newPosition = selectedPos;

        while (newPosition == selectedPos)
            newPosition = attackPositions[Random.Range(0, attackPositions.Length - 1)].transform.position;

        SwitchToNextState(SuperRMState.ChangePos);
        return newPosition;
    }

    void MoveToPosition()
    {
        Vector3 targetDir = selectedPos - transform.position;
        FaceTarget(targetDir);

        transform.position = Vector3.MoveTowards(transform.position, selectedPos, flySpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, selectedPos) <= 0)
            SwitchToNextState(SuperRMState.Attack);
    }
    protected void SwitchToNextState(SuperRMState nextState)
    {
        _currentState = nextState;
    }

    public override void TakeDamage(float amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            GameManager.instance.updatGameGoal(-1);
            animator.SetBool("Dead", true);
            isDead = true;

            StopAllCoroutines();
            //Instantiate(ammoDrop);
        }
        else
        {
            animator.SetTrigger("Damage");
            StartCoroutine(FlashDamage(Color.red));
        }
    }
}
