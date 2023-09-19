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
    public int currentAmmo;
    public int maxAmmo;

    public GameObject model;
    public ParticleSystem hitEffect;
    public AudioClip gunSound;

    

}
