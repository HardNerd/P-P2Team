using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] GunStats stats;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(stats.ammoCarried < stats.maxAmmoCarried)
            {
                stats.ammoCarried += Random.Range(1, stats.magSize);
                if(stats.ammoCarried + Random.Range(1, stats.magSize) > stats.maxAmmoCarried)
                {
                    stats.ammoCarried = stats.maxAmmoCarried;
                }
            }
            Destroy(gameObject);
        }
    }
}
