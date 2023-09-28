using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseGrenade : MonoBehaviour
{
    [SerializeField] int destroySpeed;
    [SerializeField] public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(explode());
    }

    IEnumerator explode()
    {
        yield return new WaitForSeconds(destroySpeed);
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(gameObject);
    }
}
