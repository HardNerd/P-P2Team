using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [SerializeField] GunStats stats;


    void Start()
    {
        stats.loadedAmmo = stats.magSize;
        //stats.ammoCarried = stats.maxAmmoCarried;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.instance.playerGunScript.GunPickup(stats);

            Destroy(gameObject);

        }
    }

}
