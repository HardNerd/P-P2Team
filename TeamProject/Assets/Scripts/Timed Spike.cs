using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSpike : MonoBehaviour
{
    public float Damage = 2f;
    private bool isSpike = false;
    [SerializeField] AudioSource outSound;
    [SerializeField] AudioSource inSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isSpike)
            {
                StartCoroutine(Spike());
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isSpike)
        {
            StartCoroutine(Spike());
        }
    }

    IEnumerator Spike()
    {
        isSpike = true;
        GameManager.instance.PlaySound(outSound);
        transform.position += new Vector3(0, 2, 0);
        GameManager.instance.playerController.TakeDamage(Damage);
        yield return new WaitForSeconds(1f);
        GameManager.instance.PlaySound(inSound);
        transform.position -= new Vector3(0, 2, 0);
        yield return new WaitForSeconds(1f);
        isSpike = false;
    }

}

