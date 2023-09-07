using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMarksmen : MonoBehaviour, IDamage , IPhysics
{
    [SerializeField] Renderer rendModel;
    [SerializeField] NavMeshAgent meshAgent;
    [SerializeField] Transform shootingPosition;

    [SerializeField] int HP;
    [SerializeField] int faceSpeed;

    [SerializeField] float shootingRate;
    [SerializeField] GameObject bullet;

    Vector3 playerDirection;
    bool playerInRange;
    bool Shooting;

    void Start()
    {
        GameManager.instance.updatGameGoal(1);
    }

    
    void Update()
    {
        if (playerInRange)
        {
            playerDirection = GameManager.instance.player.transform.position - transform.position;

            if (meshAgent.remainingDistance <= meshAgent.stoppingDistance)
            {
                faceTarget();

                if (!Shooting)
                {
                    StartCoroutine(shooting());
                }
            }

            meshAgent.SetDestination(GameManager.instance.player.transform.position);

        }
    }

    IEnumerator shooting()
    {
        Shooting = true;
        Instantiate(bullet, shootingPosition.position, transform.rotation);
        yield return new WaitForSeconds(shootingRate);
        Shooting = false;
    }

    public void takeDamage(int damageAmount)
    {
        HP -= damageAmount;
        StartCoroutine(damageFlash());
        if (HP <= 0)
        {
            GameManager.instance.updatGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator damageFlash()
    {
        rendModel.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rendModel.material.color = Color.blue;
    }

    void faceTarget()
    {
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * faceSpeed);
    }

    public void physics(Vector3 direction)
    {
        meshAgent.velocity += (direction / 2);
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
