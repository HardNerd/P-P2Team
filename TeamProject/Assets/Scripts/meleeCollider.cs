using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeCollider : MonoBehaviour
{
    [SerializeField] meleeEnemyAI enemy;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<IDamage>().TakeDamage(enemy.AttackDamage);
    }
}
