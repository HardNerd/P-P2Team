using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Animator animator;
    



    void Start()
    {
       

    }

    void Update()
    {
        if (Input.GetButton("Reload") && !GameManager.instance.playerController.isReloading && !GameManager.instance.isPause && GameManager.instance.playerController.GunList.Count > 0)
        {
            StartCoroutine(reload());
            return;
        }
    }

    IEnumerator reload()
    {
        GunStats currentGun = GameManager.instance.playerController.GunList[GameManager.instance.playerController.selectedGun];
        if (currentGun.loadedAmmo < currentGun.ammoCarried)
        {
            GameManager.instance.playerController.isReloading = true;
            GameManager.instance.ammoUpdate(0, 0, true);
            animator.SetBool("Reloading", true);

            yield return new WaitForSeconds(currentGun.reloadTime);
            currentGun.loadedAmmo = currentGun.magSize;
            

            animator.SetBool("Reloading", false);
            GameManager.instance.playerController.isReloading = false;
            GameManager.instance.ammoUpdate(currentGun.loadedAmmo, currentGun.ammoCarried);
        }
    }



}
