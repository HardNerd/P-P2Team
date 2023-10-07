using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static superRocketMan;

public class finalBoss : superHeavyGunner
{
    public enum Stage
    {
        Flying,
        FallToPlatform,
        Rocketman,
        AddGrenadier,
        AddZombies,
        DropDown,
        HeavyOnly
    }
    //public enum BossState
    //{
    //    Idle
    //}
    public enum SuperRMState
    {
        Attack,
        DetermineNextPos,
        ChangePos
    }

    public enum FTPState
    {
        DetermineNewPos,
        MoveToPlatformXY,
        DropToPlatform
    }

    [Header("----- BOSS States -----")]
    [SerializeField] Stage _stage;
    //[SerializeField] BossState _bossState;
    [SerializeField] SuperRMState _flyingState;
    [SerializeField] FTPState _ftpState;

    [Header("----- BOSS Attack Stats -----")]
    [SerializeField] GameObject rocketPrefab;
    [SerializeField] float rocketShootRate;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletShootRate;
    [SerializeField] GameObject molotovPrefab;
    [SerializeField] float molotovShootRate;

    [SerializeField] int maxRocketShots;

    [Header("----- Flying stats -----")]
    [SerializeField] GameObject[] attackPositions;
    [SerializeField] Transform centerPlatform;
    [SerializeField] float flySpeed;

    bool firstShot = true;
    int shots;
    Vector3 selectedPos;
    bool isWaiting;
    bool isInvinsible;
    bool madeFinalDrop;
    bool usesShield;

    float stage2HP;
    float stage3HP;
    float stage4HP;
    float stage5HP;
    float stage6HP;

    void Start()
    {
        maxHP = HP;
        shieldHPMax = shieldHP;
        usesShield = false;

        stage2HP = maxHP * (5.0f / 6.0f);
        stage3HP = maxHP * (3.0f / 4.0f);
        stage4HP = maxHP * (2.0f / 3.0f);
        stage5HP = maxHP * (1.0f / 2.0f);
        stage6HP = maxHP * (1.0f / 3.0f);

        bullet = rocketPrefab;
        shootRate = rocketShootRate;
        shots = 0;
        enemyBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isDead)
        {
            switch (_stage)
            {
                case Stage.Flying: // has full HP
                    SuperRMStates();
                    break;
                case Stage.FallToPlatform: // has 5/6 HP
                    FTPStates();
                    break;
                case Stage.Rocketman: // has 3/4 HP
                    enemyBody.isKinematic = true;
                    Attack();
                    break;
                case Stage.AddGrenadier: // has 2/3 HP
                    break;
                case Stage.AddZombies: // has 1/2 HP
                    break;
                case Stage.DropDown: // has 1/3 HP
                    break;
                case Stage.HeavyOnly: // has 1/3 HP
                    break;
                default:
                    break;
            }
        }
    }

    void SuperRMStates()
    {
        switch (_flyingState)
        {
            case SuperRMState.Attack:
                LimitedAttack();
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

    void FTPStates()
    {
        switch (_ftpState)
        {
            case FTPState.DetermineNewPos:
                isInvinsible = true;
                flySpeed = 10;
                selectedPos = new Vector3(centerPlatform.position.x, transform.position.y, centerPlatform.position.z);
                SwitchFTPState(FTPState.MoveToPlatformXY);
                break;
            case FTPState.MoveToPlatformXY:
                MoveToSelectedPos();
                break;
            case FTPState.DropToPlatform:
                DropToPlatform();
                break;
            default:
                break;
        }
    }

     void ShieldAttackStates()
    {
        switch (_currentState)
        {
            case State.Attack:
                Attack();
                break;
            case State.Staggered:
                StartCoroutine(Staggered());
                break;
            default:
                break;
        }
    }

    void SwitchStage(Stage newStage)
    {
        _stage = newStage;
    }
    
    void SwitchFlyState(SuperRMState nextState)
    {
        _flyingState = nextState;
    }

    void SwitchFTPState(FTPState nextState)
    {
        _ftpState = nextState;
    }
    
    IEnumerator Wait()
    {
        if (!isWaiting)
        {
            isWaiting = true;
            yield return new WaitForSeconds(0.5f);
            SwitchFlyState(SuperRMState.Attack);
            isWaiting = false;
        }
    }

    void MovingAttack()
    {
        MoveEnemy();

        if (!isShooting)
            StartCoroutine(base.shoot());
    }

    void LimitedAttack()
    {
        if (shots >= maxRocketShots)
        {
            shots = 0;
            firstShot = true;
            SwitchFlyState(SuperRMState.DetermineNextPos);
            return;
        }

        Attack();
    }

    // override to use this class's shoot method
    protected override void Attack() // override unnecessary?
    {
        playerDirection = GameManager.instance.player.transform.position - headPos.position;
        FaceTarget(playerDirection);

        // Wait for enemy to face target
        if (firstShot)
        {
            StartCoroutine(Wait());
            firstShot = false;
        }

        if (!isShooting && !isWaiting)
            StartCoroutine(shoot());
    }

    protected override IEnumerator shoot()
    {
        isShooting = true;
        shots++;
        animator.SetTrigger("Shoot");
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    Vector3 ChooseRandomPos()
    {
        Vector3 newPosition = selectedPos;

        while (newPosition == selectedPos)
            newPosition = attackPositions[Random.Range(0, attackPositions.Length - 1)].transform.position;

        SwitchFlyState(SuperRMState.ChangePos);
        return newPosition;
    }

    void MoveToPosition()
    {
        Vector3 targetDir = selectedPos - transform.position;
        FaceTarget(targetDir);

        transform.position = Vector3.MoveTowards(transform.position, selectedPos, flySpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, selectedPos) <= 0)
            SwitchFlyState(SuperRMState.Attack);
    }
    void MoveToSelectedPos()
    {
        Vector3 targetDir = selectedPos - transform.position;
        FaceTarget(targetDir);

        transform.position = Vector3.MoveTowards(transform.position, selectedPos, flySpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, selectedPos) <= 0)
            SwitchFTPState(FTPState.DropToPlatform);
    }

    void DropToPlatform()
    {
        enemyBody.isKinematic = false;

        if ((int)Vector3.Distance(transform.position, centerPlatform.position) <= 0)
        {
            isInvinsible = false;
            HP = stage3HP;
            StartCoroutine(FlashDamage(Color.red));
            SwitchStage(Stage.Rocketman);
        }
    }

    public override void TakeDamage(float amount)
    {
        if (isInvinsible)
            return;

        if (!isStaggered && usesShield)
        {
            shieldHP -= amount;

            if (shieldHP <= 0)
                SwitchToNextState(State.Staggered);
            else
                FlashDamage(Color.blue);
        }
        else
        {
            HP -= amount;

            if (HP <= 0)
            {
                GameManager.instance.updatGameGoal(-1);
                animator.SetBool("Dead", true);
                isDead = true;

                StopAllCoroutines();
                //Instantiate(ammoDrop);
                return;
            }
            else
            {
                animator.SetTrigger("Damage");
                StartCoroutine(FlashDamage(Color.red));
            }
        }

        // Change Stage
        if (HP <= stage6HP)
        {
            if (!madeFinalDrop)
                SwitchStage(Stage.DropDown);
            else 
                SwitchStage(Stage.HeavyOnly);
        }
        else if (HP <= stage5HP) SwitchStage(Stage.AddZombies);
        else if (HP <= stage4HP) SwitchStage(Stage.AddGrenadier);
        else if (HP <= stage3HP) SwitchStage(Stage.Rocketman);
        else if (HP <= stage2HP) SwitchStage(Stage.FallToPlatform);
    }
}
