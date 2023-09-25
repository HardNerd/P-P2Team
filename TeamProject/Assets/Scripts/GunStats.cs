using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class GunStats : ScriptableObject
{
    public float shootRate;
    public float shootDamage;
    public int shootDistance;
    public int loadedAmmo; // ammo currently loading. Variable name Change: loadedAmmo
    public int maxAmmoCarried; // max ammo that can be carried by player depending on gun type. Variable name Change: "maxAmmoCarried"
    public int magSize; // size of mag
    public int ammoCarried; // total ammo carried by player

    public float reloadTime;

    public GameObject model;
    public ParticleSystem hitEffect;
    public AudioClip gunSound;

    [SerializeField] int toughnessDmg;
    

}
