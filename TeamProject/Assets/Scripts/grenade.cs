using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] int speed;
    [SerializeField] int destroySpeed;
    [SerializeField] public GameObject explosion;

    void Start()
    {
        rb.velocity = (GameManager.instance.player.transform.position - transform.position).normalized * speed;
        StartCoroutine(explode());
    }

    IEnumerator explode()
    {
        yield return new WaitForSeconds(destroySpeed);
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(gameObject);

    }
    //test
}
