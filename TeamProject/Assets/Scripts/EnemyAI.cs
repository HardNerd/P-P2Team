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
    [SerializeField] protected float animChangeSpeed;
    [SerializeField] float stopAtDamageTime;

    protected Vector3 playerDirection;
    protected float angleToPlayer;
    protected Vector3 startingPos;
    protected float speedOrig = 0; // you have to set it in the child classes
    protected bool playerInSight;
    protected bool isDead = false;
    
    protected void MoveEnemy()
    {
        playerDirection = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    FaceTarget();
                    playerInSight = true;
                }
                else
                    playerInSight = false;
            }
        }
        agent.SetDestination(GameManager.instance.player.transform.position);
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
}
