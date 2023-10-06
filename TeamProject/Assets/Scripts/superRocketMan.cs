using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class superRocketMan : EnemyAI
{
    public enum SuperRMState
    {
        Idle,
        Attack,
        Wait,
        ChangePos
    }

    [Header("----- Shoot Stats -----")]
    [SerializeField] Transform shootPos;
    [SerializeField] float shootRate;
    [SerializeField] GameObject projectile;

    [SerializeField] GameObject[] attackPositions;

    [SerializeField] SuperRMState _currentState;

    void Start()
    {
        
    }

    void Update()
    {
        if (!isDead)
        {
            switch (_currentState)
            {
                case SuperRMState.Idle:
                    break; 
                case SuperRMState.Attack:
                    Attack();
                    break;
                case SuperRMState.Wait:
                    break;
                case SuperRMState.ChangePos:
                    break;
                default:
                    break;
            }
        }
    }

    void Attack()
    {

    }
}
