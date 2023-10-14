using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
//using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class Gun : MonoBehaviour

{
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem particles;
    [SerializeField] ParticleSystem bloodHitEffect;

    [Header("----- Gun Stats -----")]
    [SerializeField] public List<GunStats> GunList = new List<GunStats>();
    [SerializeField] float shootRate;
    [SerializeField] float shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] float reloadTime;

    [SerializeField] AudioSource shootSound;
    [SerializeField] AudioSource reloadSound;
    [SerializeField] AudioSource gunPickupSound;
    [SerializeField] AudioSource ammoPickupSound;

    private bool isShooting;
    public bool isReloading;
    public int selectedGun;
    public GunStats gunStatsGun;
    private float origPitch;


    void Start()
    {

    }

    void Update()
    {
        GunSelector();

        if (Input.GetButton("Fire1") && !isShooting && !GameManager.instance.isPause && !isReloading)
            StartCoroutine(shoot());
            

        if (Input.GetButton("Reload") && !isReloading && !GameManager.instance.isPause && GunList.Count > 0)
        {
            StartCoroutine(Reload());
            return;
        }
    }

    IEnumerator shoot()
    {
        if (GunList.Count > 0 && GunList[selectedGun].loadedAmmo > 0)
        {
            isShooting = true;
            GunList[selectedGun].loadedAmmo--;
            GameManager.instance.ammoUpdate(GunList[selectedGun].loadedAmmo, GunList[selectedGun].ammoCarried);
            GameManager.instance.AudioChange(shootSound);
            shootSound.Play();
            StartCoroutine(GameManager.instance.clipEnd(shootSound, shootSound.clip.length, origPitch));

            RaycastHit hitInfo;
            Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));

            if (Physics.Raycast(ray, out hitInfo, shootDistance))
            {
                IDamage damageable = hitInfo.collider.GetComponent<IDamage>();

                if (damageable != null)
                {
                    Instantiate(bloodHitEffect, hitInfo.point, GunList[selectedGun].hitEffect.transform.rotation);
                }
                else
                    Instantiate(GunList[selectedGun].hitEffect, hitInfo.point, GunList[selectedGun].hitEffect.transform.rotation);

                AudioSource hitsound = GunList[selectedGun].hitEffect.GetComponent<AudioSource>();
                float currPitch = hitsound.pitch;
                GameManager.instance.AudioChange(hitsound);
                hitsound.pitch = currPitch;
                damageable?.TakeDamage(shootDamage, null);
            }

            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }

    IEnumerator Reload()
    {
        GunStats currentGun = GunList[selectedGun];
        if (currentGun.loadedAmmo < currentGun.magSize && currentGun.ammoCarried > 0)
        {
            isReloading = true;
            GameManager.instance.ammoUpdate(0, 0, true);
            animator.SetBool("Reloading", true);

            GameManager.instance.PlaySound(reloadSound);
            yield return new WaitForSeconds(currentGun.reloadTime);
            if ((currentGun.magSize - currentGun.loadedAmmo) <= currentGun.ammoCarried)
            {
                currentGun.ammoCarried -= currentGun.magSize - currentGun.loadedAmmo;
                currentGun.loadedAmmo = currentGun.magSize;
            }
            else
            {
                currentGun.loadedAmmo += currentGun.ammoCarried;
                currentGun.ammoCarried = 0;
            }


            animator.SetBool("Reloading", false);
            isReloading = false;
            GameManager.instance.ammoUpdate(currentGun.loadedAmmo, currentGun.ammoCarried);

        }
    }

    public void GunPickup(GunStats gun)
    {
        GunList.Add(gun);
        shootDamage = gun.shootDamage;
        shootDistance = gun.shootDistance;
        shootRate = gun.shootRate;
        shootSound.clip = gun.gunSound;
        reloadSound.clip = gun.reloadSound;
        origPitch = shootSound.pitch;
        reloadTime = gun.reloadTime;

        gunPickupSound.clip = gun.pickupSound;
        GameManager.instance.PlaySound(gunPickupSound);

        GetComponent<MeshFilter>().sharedMesh = gun.model.GetComponent<MeshFilter>().sharedMesh;
        GetComponent<Renderer>().sharedMaterial = gun.model.GetComponent<Renderer>().sharedMaterial;

        selectedGun = GunList.Count - 1;
        GameManager.instance.ammoUpdate(GunList[selectedGun].loadedAmmo, GunList[selectedGun].ammoCarried);
    }

    void GunSelector()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < GunList.Count - 1)
        {
            selectedGun++;
            GunChange();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            GunChange();
        }
    }

    void GunChange()
    {
        shootDamage = GunList[selectedGun].shootDamage;
        shootDistance = GunList[selectedGun].shootDistance;
        shootRate = GunList[selectedGun].shootRate;
        shootSound.clip = GunList[selectedGun].gunSound;
        reloadSound.clip = GunList[selectedGun].reloadSound;
        origPitch = shootSound.pitch;
        reloadTime = GunList[selectedGun].reloadTime;
        GameManager.instance.ammoUpdate(GunList[selectedGun].loadedAmmo, GunList[selectedGun].ammoCarried);

        GetComponent<MeshFilter>().sharedMesh = GunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        GetComponent<Renderer>().sharedMaterial = GunList[selectedGun].model.GetComponent<Renderer>().sharedMaterial;
    }

    public void AmmoPickup(GunStats gunStats, AudioClip pickupSound)
    {
        if (gunStats.ammoCarried < gunStats.maxAmmoCarried)
        {
            ammoPickupSound.clip = pickupSound;
            GameManager.instance.PlaySound(ammoPickupSound);
            gunStats.ammoCarried += UnityEngine.Random.Range(1, gunStats.magSize);
            if (gunStats.ammoCarried + UnityEngine.Random.Range(1, gunStats.magSize) > gunStats.maxAmmoCarried)
            {
                gunStats.ammoCarried = gunStats.maxAmmoCarried;
            }
        }

        GameManager.instance.ammoUpdate(GunList[selectedGun].loadedAmmo, GunList[selectedGun].ammoCarried);
    }
}