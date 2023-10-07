using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class GunPickup : MonoBehaviour, IDataPersistence
{
    [SerializeField] GunStats stats;


    

    void Start()
    {
        stats.loadedAmmo = stats.magSize;
        stats.ammoCarried = stats.maxAmmoCarried;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !stats.collected)
        {
            GameManager.instance.playerGunScript.GunPickup(stats);
            
            stats.collected = true;

            Destroy(gameObject);

        }
    }

    void IDataPersistence.LoadData(GameData data)
    {
        if (stats.collected == true)
        {
            gameObject.SetActive(false);
        }
        
    }

    void IDataPersistence.SaveData(GameData data)
    {
        
    }
}
