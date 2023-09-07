using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] int dmg;
    [SerializeField] int velocity;
    [SerializeField] int destroybullet;

    void Start()
    {
        rb.velocity = transform.forward * velocity;
        Destroy(gameObject, destroybullet);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage damageable = other.GetComponent<IDamage>();
        if (damageable != null)
        {
            damageable.takeDamage(dmg);
        }
        Destroy(gameObject);
    }
}
