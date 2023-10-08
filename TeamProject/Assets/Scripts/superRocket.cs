using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class superRocket : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamage damageable = other.GetComponent<IDamage>();
        if (damageable != null)
            damageable.TakeDamage(damage);

        Explode();
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(gameObject);
    }
}
