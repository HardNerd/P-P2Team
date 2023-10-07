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

    [Header("----- Shield Stats -----")]
    [SerializeField] protected float shieldHP;
    [SerializeField] int staggerTime;

    [SerializeField] protected State _currentState;

    protected float shieldHPMax;
    protected bool isStaggered;

    void Start()
    {
        shieldHPMax = shieldHP;
        _currentState = State.Attack;
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

            // Change color to show staggered mode (Player can now attack)
            // Color change will change to animation in the future
            Color origColor = models[0].material.color;
            for (int i = 0; i < models.Length; i++)
                models[i].material.color = Color.grey;

            yield return new WaitForSeconds(staggerTime);
            for (int i = 0; i < models.Length; i++)
                models[i].material.color = origColor;

            shieldHP = shieldHPMax;
            SwitchToNextState(State.Attack);
            isStaggered = false;
        }
    }

    protected void SwitchToNextState(State nextState)
    {
        _currentState = nextState;
    }

    public override void TakeDamage(float amount)
    {
        if (isStaggered)
            base.TakeDamage(amount);
        else
        {
            shieldHP -= amount;
            //FlashDamage(Color.red);

            if (shieldHP <= 0)
                SwitchToNextState(State.Staggered);
            else
                FlashDamage(Color.blue);
        }
    }

    public override void physics(Vector3 direction)
    {
        return;
    }
}
