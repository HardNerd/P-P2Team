using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gysers : MonoBehaviour
{
    public float Damage = 2f;
    private bool IsGeyser = false;
    [SerializeField] private ParticleSystem particles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!IsGeyser)
            {
                StartCoroutine(Geyser());
            }
        }
    }

   

    IEnumerator Geyser()
    {
        IsGeyser = true;
        GameManager.instance.playerController.TakeDamage(Damage);
        Instantiate(particles);
        yield return new WaitForSeconds(5f);
        IsGeyser = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsGeyser)
        {
            StartCoroutine(Geyser());
        }
    }
}
