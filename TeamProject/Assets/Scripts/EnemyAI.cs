using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage, IPhysics
{
    [Header("----- Components -----")]
    [SerializeField] protected Renderer[] models;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform headPos;
    [SerializeField] protected Transform B_footR;
    [SerializeField] protected Collider hitBox;
    [SerializeField] protected Collider meleeCollider;
    [SerializeField] protected GameObject ammoDrop;

    [Header("----- Enemy Stats -----")]
    [SerializeField] protected float HP;
    [SerializeField] protected int targetFaceSpeed;
    //[SerializeField] float viewAngle;
    [SerializeField] protected float animChangeSpeed;
    [SerializeField] protected float stopAtDamageTime;
    [SerializeField] bool isPushable;
    [SerializeField] bool reactsToHit;
    [SerializeField] int pushBackResolve;
    [SerializeField] protected float agentSpeed;

    protected Vector3 playerDirection;
    protected float angleToPlayer;
    //protected Vector3 startingPos;
    protected float speedOrig = 0; // you have to set it in the child classes
    protected bool isDead = false;
    private Vector3 pushBack;
    protected Rigidbody enemyBody;

    protected float maxHP;

    protected virtual void MoveEnemy()
    {
        playerDirection = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);
        
        if (agent.remainingDistance <= agent.stoppingDistance)
            FaceTarget(playerDirection);
        
        agent.SetDestination(GameManager.instance.player.transform.position);
    }

    virtual public void TakeDamage(float amount, string source)
    {
        HP -= amount;
        
        if (meleeCollider != null)
            meleeColliderOff();

        if (HP <= 0)
        {
            GameManager.instance.updatGameGoal(-1);
            Instantiate(ammoDrop, B_footR.position, Quaternion.identity);
            hitBox.enabled = false;
            agent.enabled = false;
            animator.SetBool("Dead", true);
            isDead = true;

            StopAllCoroutines();
           
        }
        else
        {
            animator.SetTrigger("Damage");
            StartCoroutine(FlashDamage(Color.red));
            if (reactsToHit)
                StartCoroutine(StopMoving());

            if (pushBack.magnitude > 0.01f && isPushable)
            {
                Vector3 direction = (transform.position - pushBack).normalized;
                enemyBody.AddForce(direction * pushBackResolve);
            }

        }

    }
    protected IEnumerator FlashDamage(Color flashColor)
    {
        Color origColor = models[0].material.color;
        for (int i = 0; i < models.Length; i++)
            models[i].material.color = flashColor;

        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < models.Length; i++)
            models[i].material.color = origColor;
    }

    virtual protected IEnumerator StopMoving()
    {
        agent.speed = 0;
        yield return new WaitForSeconds(stopAtDamageTime);
        agent.speed = speedOrig;
    }

    protected void FaceTarget(Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * targetFaceSpeed);
    }

    virtual public void physics(Vector3 direction)
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
