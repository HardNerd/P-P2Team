using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage, IPhysics
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform headPos;
    [SerializeField] protected Collider hitBox;

    [Header("----- Enemy Stats -----")]
    [SerializeField] float HP;
    [SerializeField] int targetFaceSpeed;
    [SerializeField] float viewAngle;
    [SerializeField] bool canRoam;
    [SerializeField] int roamDistance;
    [SerializeField] int roamPauseTime;
    [SerializeField] bool hasGuardAnim;
    [SerializeField] protected float animChangeSpeed;
    [SerializeField] float stopAtDamageTime;

    protected Vector3 playerDirection;
    protected bool playerInRange;
    protected float angleToPlayer;
    protected Vector3 startingPos;
    protected float stoppingDistanceOrig = 0; // you have to set it in the child classes
    protected float speedOrig = 0; // you have to set it in the child classes
    protected bool playerInSight;
    protected bool isDead = false;
    bool destinationChosen;

    protected void MoveEnemy()
    {
        if (playerInRange && !CanSeePlayer() && canRoam)
            StartCoroutine(Roam());
        else if (!playerInRange && canRoam)
            StartCoroutine(Roam());
    }

    IEnumerator Roam()
    {
        if (agent.remainingDistance < 0.05f && !destinationChosen)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamPauseTime);

            Vector3 randomPos = Random.insideUnitSphere * roamDistance;
            randomPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDistance, 1);
            agent.SetDestination(hit.position);

            destinationChosen = false;
        }
    }

    protected bool CanSeePlayer()
    {
        playerDirection = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.SetDestination(GameManager.instance.player.transform.position);
                agent.stoppingDistance = stoppingDistanceOrig;

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    FaceTarget();

                    playerInSight = true;
                }
                else
                    playerInSight = false;

                if (hasGuardAnim)
                    animator.SetBool("PlayerInSight", playerInSight);

                return true;
            }
        }
        return false;
    }

    public void TakeDamage(float amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            GameManager.instance.updatGameGoal(-1);
            hitBox.enabled = false;
            agent.enabled = false;
            animator.SetBool("Dead", true);
            isDead = true;
        }
        else
        {
            animator.SetTrigger("Damage");
            agent.SetDestination(GameManager.instance.player.transform.position);

            StartCoroutine(FlashDamage());
            StartCoroutine(StopMoving());

        }

    }
    IEnumerator FlashDamage()
    {
        Color origColor = model.material.color;

        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = origColor;
    }

    IEnumerator StopMoving()
    {
        agent.speed = 0;
        yield return new WaitForSeconds(stopAtDamageTime);
        agent.speed = speedOrig;
    }

    void FaceTarget()
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
            agent.stoppingDistance = 0;
        }
    }

}
