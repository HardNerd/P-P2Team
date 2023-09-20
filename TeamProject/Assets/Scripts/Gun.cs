using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Animator animator;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetButton("Reload") && !GameManager.instance.playerController.isReloading && !GameManager.instance.isPause)
        {
            StartCoroutine(reload());
            return;
        }
    }

    IEnumerator reload()
    {
        GunStats currentGun = GameManager.instance.playerController.GunList[GameManager.instance.playerController.selectedGun];
        if (currentGun.currentAmmo < currentGun.maxAmmo)
        {
            GameManager.instance.playerController.isReloading = true;
            animator.SetBool("Reloading", true);

            yield return new WaitForSeconds(currentGun.reloadTime);
            currentGun.currentAmmo = currentGun.maxAmmo;

            animator.SetBool("Reloading", false);
            GameManager.instance.playerController.isReloading = false;
        }
    }
}
