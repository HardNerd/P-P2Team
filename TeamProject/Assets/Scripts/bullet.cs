using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] int dmg;
    [SerializeField] int velocity;
    [SerializeField] int destroyBulletTime;

    void Start()
    {
        rb.velocity = (GameManager.instance.player.transform.position - transform.position).normalized * velocity;
        Destroy(gameObject, destroyBulletTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage damageable = other.GetComponent<IDamage>();
        if (damageable != null)
        {
            damageable.TakeDamage(dmg);
        }
        Destroy(gameObject);
    }
}
