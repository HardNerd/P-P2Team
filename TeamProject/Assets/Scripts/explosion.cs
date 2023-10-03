using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [SerializeField] int explosionSize;
    [SerializeField] GameObject explosionparticle;
    [SerializeField] int damage;
    [SerializeField] float explosionTime;

    private void Start()
    {
        Instantiate(explosionparticle, transform.position, explosionparticle.transform.rotation);
        Destroy(gameObject, explosionTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        if (other.TryGetComponent<IPhysics>(out var isphysicpossible))
        {
            isphysicpossible.physics((other.transform.position - transform.position).normalized * explosionSize);
        }

        if (other.TryGetComponent<IDamage>(out var damageable))
        {
            damageable.TakeDamage(damage);
        }
    }
}
