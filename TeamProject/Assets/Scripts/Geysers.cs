using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gysers : MonoBehaviour
{
    public float Damage = 2f;
    private bool IsGeyser = false;

    private void Update()
    {
       
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.position += new Vector3(0, 3, 0);
            if (!IsGeyser)
            {
                StartCoroutine(Geyser());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.position -= new Vector3(0, 3, 0);
        }
    }


    IEnumerator Geyser()
    {
        IsGeyser = true;
        GameManager.instance.playerController.TakeDamage(Damage);
        yield return new WaitForSeconds(5f);
        IsGeyser = false;
    }
}
