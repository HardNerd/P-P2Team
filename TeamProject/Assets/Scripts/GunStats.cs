using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu]

public class GunStats : ScriptableObject
{
    public float shootRate;
    public float shootDamage;
    public int shootDistance;
    
    public int loadedAmmo; // ammo currently loaded
    public int maxAmmoCarried; // max ammo that can be carried by player depending on gun type.
    public int magSize; // size of mag
   
    public int ammoCarried; // total ammo carried by player

    public float reloadTime;
    public int gunID;
    public GameObject model;
    public GameObject ammoType;
    public ParticleSystem hitEffect;
    public AudioClip gunSound;
    public AudioClip reloadSound;
    public AudioClip pickupSound;

    public int toughnessDmg;
    public bool collected = false;
}
