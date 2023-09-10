using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeavyGunnerAI : MonoBehaviour, IDamage, IPhysics
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;


    [SerializeField] int HP;
    [SerializeField] int targetFaceSpeed;
    [SerializeField] float viewAngle;

    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;

    Vector3 playerDirection;
    float angleToPlayer;
    bool playerInRange;
    bool isShooting;

    void Start()
    {
        GameManager.instance.updatGameGoal(1);
    }

    void Update()
    {
        if (playerInRange && CanSeePlayer())
        {
            Debug.Log(playerInRange);
            Debug.Log(agent.remainingDistance);
            playerDirection = GameManager.instance.player.transform.position - transform.position;
            faceTarget();
            agent.SetDestination(GameManager.instance.player.transform.position);
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if(!isShooting)
                    StartCoroutine(shoot());
            }
        }

    }

    bool CanSeePlayer()
    {
        playerDirection = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);
        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDirection);

        RaycastHit hit;
        if(Physics.Raycast(headPos.position, playerDirection, out hit))
        {
            if(hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                return true;
            }
        }    

        return false;
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        StartCoroutine(FlashDamage());
        agent.SetDestination(GameManager.instance.player.transform.position);

        if (HP <= 0)
        {
            GameManager.instance.updatGameGoal(-1);
            Destroy(gameObject);
        }

    }
    IEnumerator FlashDamage()
    {
        Color origColor = model.material.color;

        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = origColor;
    }

    void faceTarget()
    {
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * targetFaceSpeed);
    }

    public void physics(Vector3 direction)
    {
        agent.velocity += (direction / 2);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

}
