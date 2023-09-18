using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : grenade
{
    [SerializeField] int damage;


    IEnumerator instantExplode()
    {
        yield return new WaitForSeconds(0);
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        IDamage damageable = other.GetComponent<IDamage>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
        StartCoroutine(instantExplode());
    }
}
