using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public float Damage = 2f;
    private bool isSpike = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            transform.position += new Vector3(0,2,0);
            if(!isSpike)
            {
                StartCoroutine(Spike());
            }
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            transform.position -= new Vector3(0, 2, 0);
        }
        
    }

    IEnumerator Spike()
    {
        isSpike = true;
        GameManager.instance.playerController.TakeDamage(Damage);
        yield return new WaitForSeconds(1f);
        isSpike = false;
    }

}
