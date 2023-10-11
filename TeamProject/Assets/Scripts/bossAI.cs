using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bossAI : EnemyAI
{
    [SerializeField] GameObject[] roomDoors;
    [SerializeField] Transform[] coverPositions;
    [SerializeField] Transform shootPos;
    [SerializeField] LineRenderer laserSight;
    [SerializeField] float shootRate;
    [SerializeField] float aimDuration;
    [SerializeField] int timeInCover;

    [SerializeField] GameObject bullet;

    [Header("----- BOSS Power Up -----")]
    [SerializeField] Transform dropLocation;

    public static bool inCover = false;
    private bool isAiming = false;
    private float aimTimer = 0f;
    private float shootTimer = 0f;

    void Start()
    {
        B_footR = dropLocation;
        agent.SetDestination(coverPositions[0].transform.position);
        laserSight.enabled = false;
    }
    void Update()
    {
        if (!isDead)
        {
            StartCoroutine(TakeCover());
            if (inCover)
            {
                if(!isAiming)
                {
                    AimAtPlayer();
                }
                else
                {
                    aimTimer += Time.deltaTime;
                    if(aimTimer>=aimDuration)
                    {
                        Shoot();
                    }
                }
            }
        }
    }

    IEnumerator TakeCover()
    {
        int selectedCoverPosition = Random.Range(0, (coverPositions.Length));
        if(!inCover && agent.remainingDistance == 0)
        {
            inCover = true;
            yield return new WaitForSeconds(timeInCover);
            agent.SetDestination(coverPositions[selectedCoverPosition].transform.position);
            inCover = false;
            
        }
    }

    private void AimAtPlayer()
    {
        //turn towards player
        playerDirection = GameManager.instance.player.transform.position - transform.position;
        FaceTarget(playerDirection);
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);
        transform.rotation = Quaternion.AngleAxis(angleToPlayer, Vector3.forward);

        //Turn on laser
        laserSight.enabled = true;
        laserSight.SetPosition(0, shootPos.transform.position-transform.position);

        //Make laser endpoint
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDirection, Mathf.Infinity);
        if(hit.collider != null)
        {
            laserSight.SetPosition(1, hit.point);
        }
        else
        {
            laserSight.SetPosition(1, GameManager.instance.player.transform.position - transform.position);
        }
        isAiming = true;
    }

    private void Shoot()
    {
        shootTimer += Time.deltaTime;
        if(shootTimer >= shootRate)
        {
            GameObject newBullet = Instantiate(bullet, shootPos.transform.position, transform.rotation);
            //Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
            //rb.velocity = (GameManager.instance.player.transform.position - transform.position).normalized * 10f;

            isAiming = false;
            aimTimer = 0f;
            shootTimer = 0f;

            laserSight.enabled = false;
        }
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
