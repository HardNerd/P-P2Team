using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage, IPhysics
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPos;


    [SerializeField] int HP;
    [SerializeField] int targetFaceSpeed;

    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;

    Vector3 playerDirection;
    Vector3 pushBack;
    bool playerInRange;
    bool isShooting;

    void Start()
    {
        GameManager.instance.updatGameGoal(1);
    }

    void Update()
    {
        if (playerInRange)
        {
            playerDirection = GameManager.instance.player.transform.position - transform.position;
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                faceTarget();

                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }

            }
            agent.SetDestination(GameManager.instance.player.transform.position);
        }

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

        if (HP <= 0)
        {
            GameManager.instance.updatGameGoal(-1);
            Destroy(gameObject);
        }

    }
    IEnumerator FlashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
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
