using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
//using UnityEditor.SceneTemplate;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
   
    [SerializeField]  GunStats gunStats;
    public AudioClip ammoPickupSound;
   
    

    private void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Player"))
        {
            Gun gunScript = GameManager.instance.playerGunScript;

            for (int i = 0; i < gunScript.GunList.Count; i++)
            {
                if (gunStats.gunID == gunScript.GunList[i].gunID && gunStats.ammoCarried < gunStats.maxAmmoCarried)
                {
                    Destroy(gameObject);
                    GameManager.instance.playerGunScript.AmmoPickup(gunStats);
                   
                }
            }
           
        }

    }
}
