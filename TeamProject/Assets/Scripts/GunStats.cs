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
    public int loadedAmmo; // ammo currently loading
    //public int maxAmmoCarried; // max ammo that can be carried by player depending on gun type.
<<<<<<< HEAD
    public int mag;
=======
>>>>>>> 4ebce4aad5d864b19be0b349531d6474c348200d
    public int magSize; // size of mag
    public int ammoCarried; // total ammo carried by player

    public float reloadTime;

    public GameObject model;
    public GameObject ammoType;
    public ParticleSystem hitEffect;
    public AudioClip gunSound;

    public int toughnessDmg;
   


}
