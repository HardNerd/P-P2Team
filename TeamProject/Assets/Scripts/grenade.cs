using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : baseGrenade
{
    [SerializeField] Rigidbody rb;
    [SerializeField] int speed;
    void Start()
    {
        rb.velocity = (GameManager.instance.player.transform.position - transform.position).normalized * speed;
    }
    
}
