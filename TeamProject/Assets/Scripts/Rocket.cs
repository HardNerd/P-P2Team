using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : grenade
{
    [SerializeField] int damage;

    private void OnTriggerEnter(Collider other)
    {
        IDamage damageable = other.GetComponent<IDamage>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage, "Rocketed Away");
        }
        StartCoroutine(instantExplode());
    }
    IEnumerator instantExplode()
    {
        yield return new WaitForSeconds(0);
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(gameObject);
    }
}
