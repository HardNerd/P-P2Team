using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static grenadierAI;

public class superHeavyGunner : defaultEnemy
{
    public enum State
    {
        Attack,
        Staggered
    }
    [SerializeField] GameObject[] roomDoors;
    [SerializeField] private GameObject bossTriggerToggle;
    [Header("----- Shield Stats -----")]
    [SerializeField] protected float shieldHP;
    [SerializeField] int staggerTime;
    [SerializeField] GameObject gun;
    [SerializeField] protected enemyHealthBar shieldBar;
    [SerializeField] protected GameObject shieldObj;

    [Header("----- STATE -----")]
    [SerializeField] protected State _currentState;

    [Header("----- BOSS Power Up -----")]
    [SerializeField] Transform dropLocation;

    protected float shieldHPMax;
    protected bool isStaggered;

    void Start()
    {
        B_footR = dropLocation;
        shieldHPMax = shieldHP;
        _currentState = State.Attack;
        shieldHPMax = shieldHP;
        maxHP = HP;
        healthBar.UpdateHealthBar(HP, maxHP);
        shieldBar.UpdateHealthBar(shieldHP, shieldHPMax);
        healthObj.SetActive(true);
    }

    void Update()
    {
        if (!isDead)
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
    }

    virtual protected void Attack()
    {
        playerDirection = GameManager.instance.player.transform.position - headPos.position;

        FaceTarget(playerDirection);

        if (!isShooting)
            StartCoroutine(shoot());
    }

    protected IEnumerator Staggered()
    {
        if (!isStaggered)
        {
            isStaggered = true;
            animator.SetBool("Staggered", true);
            gun.SetActive(false);

            // Change color to show staggered mode (Player can now attack)
            Color origColor = models[0].material.color;
            for (int i = 0; i < models.Length; i++)
                models[i].material.color = Color.grey;

            yield return new WaitForSeconds(staggerTime);
            for (int i = 0; i < models.Length; i++)
                models[i].material.color = origColor;

            shieldHP = shieldHPMax;
            shieldBar.UpdateHealthBar(shieldHP, shieldHPMax);
            SwitchToNextState(State.Attack);
            animator.SetBool("Staggered", false);
            gun.SetActive(true);
            isStaggered = false;
        }
    }

    protected void SwitchToNextState(State nextState)
    {
        _currentState = nextState;
    }

    public override void TakeDamage(float amount, string source = null)
    {
        if (isStaggered)
            base.TakeDamage(amount, null);
        else
        {
            shieldHP -= amount;
            shieldBar.UpdateHealthBar(shieldHP, shieldHPMax);

            if (shieldHP <= 0)
                SwitchToNextState(State.Staggered);
            else
                FlashDamage(Color.blue);
        }

        if (HP <= 0)
        {
            for (int i = 0; i < roomDoors.Length; i++)
                roomDoors[i].SetActive(false);
            bossTriggerToggle.SetActive(false);
        }
    }
            

    public override void physics(Vector3 direction)
    {
        return;
    }
}
