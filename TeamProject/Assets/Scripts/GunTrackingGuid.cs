using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class GunTrackingGuid : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string guid;
    [SerializeField] GunStats stats;
    [SerializeField] private int amountPickedUp;

    void Start()
    {
        stats.loadedAmmo = stats.magSize;
        stats.ammoCarried = stats.maxAmmoCarried;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !stats.collected)
        {
            GameManager.instance.playerGunScript.GunPickup(stats);

            stats.collected = true;
            amountPickedUp++;

            Destroy(gameObject);

        }
    }


    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        guid = System.Guid.NewGuid().ToString();
    }

    public void LoadData(GameData data)
    {

        amountPickedUp = data.gunPickedUpAmount;

        data.gunsCollected.TryGetValue(guid, out stats.collected);
        if (stats.collected == true)
        {
            gameObject.SetActive(false);

            if (data.gunsCollected.ContainsValue(stats.collected == true))
            {
                for (int i = 0; i < amountPickedUp; i++)
                {
                    GameManager.instance.playerGunScript.GunAddToList(stats, guid);
                }
            }
        }
    }

    public void SaveData(GameData data)
    {
        if (data.gunsCollected.ContainsKey(guid))
        {
            data.gunsCollected.Remove(guid);
        }
        data.gunsCollected.Add(guid, stats.collected);
        data.gunPickedUpAmount = amountPickedUp;
    }

    

    
}
