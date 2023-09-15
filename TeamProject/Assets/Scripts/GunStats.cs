using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class GunStats : ScriptableObject
{
    public float shootRate = 2;
    public int shootDamage = 1;
    public int shootDistance = 15;
    public int currentAmmo;
    public int maxAmmo;

    public GameObject model;
    public ParticleSystem hitEffect;
    public AudioClip gunSound;


}
