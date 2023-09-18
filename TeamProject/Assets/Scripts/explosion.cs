using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [SerializeField] int explosionSize;
    [SerializeField] GameObject explosionparticle;
    [SerializeField] int damage;

    private void Start()
    {
        Instantiate(explosionparticle, transform.position, explosionparticle.transform.rotation);
        Destroy(gameObject, 0.1f );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        IPhysics isphysicpossible = other.GetComponent<IPhysics>();

        if (isphysicpossible != null)
        {
            isphysicpossible.physics((other.transform.position - transform.position).normalized * explosionSize);
        }

        IDamage damageable = other.GetComponent<IDamage>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
}
