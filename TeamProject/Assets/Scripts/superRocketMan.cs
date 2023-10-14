using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static grenadierAI;

public class superRocketMan : EnemyAI
{
    public enum SuperRMState
    {
        ChooseTargets,
        Attack,
        Wait,
        DetermineNextPos,
        ChangePos
    }

    [SerializeField] float flySpeed;
    [SerializeField] float waitTime;

    [Header("----- Shoot Stats -----")]
    [SerializeField] ParticleSystem gunExplosion;
    [SerializeField] Transform shootPos;
    [SerializeField] float shootRate;
    [SerializeField] GameObject projectile;
    [SerializeField] int projectileSpeed;

    [SerializeField] GameObject[] attackPositions;
    [SerializeField] GameObject[] platforms;
    [SerializeField] GameObject landingZone;

    [SerializeField] SuperRMState _currentState;

    [Header("----- BOSS Power Up -----")]
    [SerializeField] Transform dropLocation;

    bool isShooting;
    Vector3 selectedPos;
    bool isWaiting;
    List<List<GameObject>> platformMatrix;
    List<GameObject> targets;
    int currentTarget;

    private void Awake()
    {
        healthBar = GetComponentInChildren<enemyHealthBar>();
    }

    void Start()
    {
        B_footR = dropLocation;
        PopulatePlatformMatrix();
        enemyBody = GetComponent<Rigidbody>();
        selectedPos = attackPositions[0].transform.position;
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
                    animator.SetBool("Idle", false);
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
        GameObject playerPlatform = null;
        for (int i = 0; i < platforms.Length; i++)
            if (platforms[i].GetComponent<rocketPlatform>().playerInside)
                playerPlatform = platforms[i];

        if (playerPlatform != null)
        {
            FindPositionInMatrix(playerPlatform, out int row, out int col);
            targets = FindNeighborCount(row, col);

            // Select random index in list to remove from target list
            int index = Random.Range(0, targets.Count - 1);
            targets.RemoveAt(index);
            // Add Player platform as a target
            targets.Add(playerPlatform);

            currentTarget = targets.Count - 1;

            SwitchToNextState(SuperRMState.Attack);
        }
    }

    void FindPositionInMatrix(GameObject platform, out int row, out int col)
    {
        for (int i = 0; i < 5; i++)
            for (int j = 0; j < 5; j++)
                if (platform == platformMatrix[i][j])
                {
                    row = i;
                    col = j;
                    return;
                }
        row = 0; col = 0;
    }

    List<GameObject> FindNeighborCount(int row, int column)
    {
        List<GameObject> neighbors = new List<GameObject>();

        for (int i = Mathf.Max(0, row - 1); i <= Mathf.Min(platformMatrix.Count - 1, row + 1); i++)
            for (int j = Mathf.Max(0, column - 1); j <= Mathf.Min(platformMatrix.Count - 1, column + 1); j++)
                if ((i, j) != (row, column))
                    if (platformMatrix[i][j] != null)
                        neighbors.Add(platformMatrix[i][j]);
        return neighbors;
    }

    void Attack()
    {
        if (targets.Count == 0)
            SwitchToNextState(SuperRMState.Wait);

        playerDirection = GameManager.instance.player.transform.position - headPos.position;
        FaceTarget(playerDirection);

        StartCoroutine(ShootTargets());
    }

    IEnumerator ShootTargets()
    {
        if (!isShooting)
        {
            isShooting = true;
            animator.SetTrigger("Shoot");
            
            // Show projectile landing zone
            Vector3 targetPos = targets[currentTarget].GetComponent<rocketPlatform>().target.position;
            Instantiate(landingZone, targetPos, landingZone.transform.rotation);

            Instantiate(gunExplosion, shootPos.position, transform.rotation);
            GameObject rocket = Instantiate(projectile, shootPos.position, Quaternion.LookRotation(targetPos - transform.position));
            Rigidbody rocket_rb = rocket.GetComponent<Rigidbody>();
            rocket_rb.velocity = (targetPos - rocket.transform.position).normalized * projectileSpeed;

            targets.RemoveAt(currentTarget);
            currentTarget--;
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }

    IEnumerator WaitAfterAttack()
    {
        playerDirection = GameManager.instance.player.transform.position - headPos.position;
        FaceTarget(playerDirection);

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
        {
            animator.SetBool("Idle", true);
            SwitchToNextState(SuperRMState.ChooseTargets);
        }
    }
    protected void SwitchToNextState(SuperRMState nextState)
    {
        _currentState = nextState;
    }

    public override void TakeDamage(float amount, string source = null)
    {
        HP -= amount;
        healthObj.SetActive(true);
        healthBar.UpdateHealthBar(HP, maxHP);

        if (HP <= 0)
        {
            GameManager.instance.updatGameGoal(-1);
            animator.SetBool("Dead", true);
            isDead = true;

            healthObj.SetActive(false);
            StopAllCoroutines();
            Instantiate(ammoDrop, B_footR.position, Quaternion.identity);
        }
        else
        {
            animator.SetTrigger("Damage");
            StartCoroutine(FlashDamage(Color.red));
        }
    }

    void PopulatePlatformMatrix()
    {
        int platformsIndx = 0;
        platformMatrix = new List<List<GameObject>>();

        for (int i = 0; i < 5; i++)
        {
            platformMatrix.Add(new List<GameObject>());
            for (int j = 0; j < 5; j++)
            {
                if ((i, j) == (0, 0) || (i, j) == (0, 2) || (i, j) == (0, 4) || (i, j) == (2, 2) || (i, j) == (4, 0) || (i, j) == (4, 2) || (i, j) == (4, 4))
                    platformMatrix[i].Add(null);
                else
                {
                    platformMatrix[i].Add(platforms[platformsIndx]);
                    platformsIndx++;
                }
            }
        }
    }
}
