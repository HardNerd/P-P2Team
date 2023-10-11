using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [SerializeField] int explosionSize;
    [SerializeField] GameObject explosionparticle;
    [SerializeField] int damage;
    [SerializeField] float explosionTime;

    bool isDamaging = false;

    private void Start()
    {
        AudioSource source = explosionparticle.GetComponent<AudioSource>();
        float currpitch = source.pitch;
        GameManager.instance.AudioChange(source);
        Instantiate(explosionparticle, transform.position, explosionparticle.transform.rotation);
        source.pitch = currpitch;
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
            damageable.TakeDamage(damage, "Blown up");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<IDamage>(out var damageable))
            StartCoroutine(fireDamage(damageable));
    }

    IEnumerator fireDamage(IDamage damageable)
    {
        if (!isDamaging)
        {
            isDamaging = true;
            yield return new WaitForSeconds(1);
            damageable.TakeDamage(damage, "Burned");
            isDamaging = false;
        }
    }
}
