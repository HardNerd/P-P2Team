using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [SerializeField] GunStats stats;


    void Start()
    {
<<<<<<< HEAD
        stats.ammoCarried = stats.magSize * stats.mag;
=======
>>>>>>> 4ebce4aad5d864b19be0b349531d6474c348200d
        stats.loadedAmmo = stats.magSize;
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
