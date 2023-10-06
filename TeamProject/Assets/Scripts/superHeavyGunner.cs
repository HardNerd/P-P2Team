using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static grenadierAI;

public class superHeavyGunner : defaultEnemy
{
    public enum SuperHGState
    {
        Attack,
        Staggered
    }

    [Header("----- Shield Stats -----")]
    [SerializeField] float shieldHP;
    [SerializeField] int staggerTime;

    [SerializeField] SuperHGState _currentState;

    float shieldHPMax;
    bool isStaggered;

    void Start()
    {
        shieldHPMax = shieldHP;
        _currentState = SuperHGState.Attack;
    }

    void Update()
    {
        if (!isDead)
        {
            switch (_currentState)
            {
                case SuperHGState.Attack:
                    Attack();
                    break;
                case SuperHGState.Staggered:
                    StartCoroutine(Staggered());
                    break;
                default:
                    break;
            }
        }
    }

    void Attack()
    {
        playerDirection = GameManager.instance.player.transform.position - headPos.position;

        FaceTarget(playerDirection);

        if (!isShooting)
            StartCoroutine(shoot());
    }

    IEnumerator Staggered()
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
            SwitchToNextState(SuperHGState.Attack);
            isStaggered = false;
        }
    }

    protected void SwitchToNextState(SuperHGState nextState)
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
                SwitchToNextState(SuperHGState.Staggered);
            else
                FlashDamage(Color.blue);
        }
    }

    public override void physics(Vector3 direction)
    {
        return;
    }
}
