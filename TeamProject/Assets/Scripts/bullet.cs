using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] protected Rigidbody rb;
    [SerializeField] int dmg;
    [SerializeField] protected float speed;
    [SerializeField] protected int destroyBulletTime;

    void Start()
    {
        rb.velocity = (GameManager.instance.player.transform.position - transform.position).normalized * speed;
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
