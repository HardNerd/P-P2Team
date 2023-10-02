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
    [HideInInspector]
    public int loadedAmmo; // ammo currently loaded
    public int maxAmmoCarried; // max ammo that can be carried by player depending on gun type.
    public int magSize; // size of mag
    [HideInInspector]
    public int ammoCarried; // total ammo carried by player

    public float reloadTime;

    public GameObject model;
    public GameObject ammoType;
    public ParticleSystem hitEffect;
    public AudioClip gunSound;

    public int toughnessDmg;
   


}
