using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public float Damage = 100f;
    
   
    private void OnTriggerEnter(Collider other)
    {
        playerController player = GameManager.instance.playerController;
        if (other.CompareTag("Player"))
        {
            player.TakeDamage(Damage);
        }
        
    }
}
