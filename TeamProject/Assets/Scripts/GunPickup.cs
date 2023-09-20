using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [SerializeField] GunStats stats;

    void Start()
    {
        stats.currentAmmo = stats.maxAmmo;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.instance.playerController.GunPickup(stats);

            Destroy(gameObject);

        }
    }

}
