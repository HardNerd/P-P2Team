using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage, IPhysics
{
    [Header("----- Components -----")]
    [SerializeField] Renderer[] models;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform headPos;
    [SerializeField] protected Collider hitBox;
    [SerializeField] protected Collider meleeCollider;

    [Header("----- Enemy Stats -----")]
    [SerializeField] float HP;
    [SerializeField] int targetFaceSpeed;
    //[SerializeField] float viewAngle;
    [SerializeField] protected float animChangeSpeed;
    [SerializeField] float stopAtDamageTime;
    [SerializeField] int pushBackResolve;

    protected Vector3 playerDirection;
    protected float angleToPlayer;
    //protected Vector3 startingPos;
    protected float speedOrig = 0; // you have to set it in the child classes
    protected bool isDead = false;
    private Vector3 pushBack;
    private Rigidbody enemyBody;

    protected virtual void MoveEnemy()
    {
        playerDirection = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);
        
        if (agent.remainingDistance <= agent.stoppingDistance)
            FaceTarget();
        
        agent.SetDestination(GameManager.instance.player.transform.position);
    }

    public void TakeDamage(float amount)
    {
        HP -= amount;

        if (pushBack.magnitude > 0.01f)
        {
            Vector3 direction = (transform.position - pushBack).normalized;
            enemyBody.AddForce(direction * pushBackResolve);
        }
        
        if (meleeCollider != null)
            meleeColliderOff();

        if (HP <= 0)
        {
            GameManager.instance.updatGameGoal(-1);
            hitBox.enabled = false;
            agent.enabled = false;
            animator.SetBool("Dead", true);
            isDead = true;

            StopAllCoroutines();
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
        Color origColor = models[0].material.color;
        for (int i = 0; i < models.Length; i++)
            models[i].material.color = Color.red;

        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < models.Length; i++)
            models[i].material.color = origColor;
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

    public void meleeColliderOn()
    {
        meleeCollider.enabled = true;
    }

    public void meleeColliderOff()
    {
        meleeCollider.enabled = false;
    }
}
