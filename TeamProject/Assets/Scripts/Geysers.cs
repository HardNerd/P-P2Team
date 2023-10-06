using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gysers : MonoBehaviour
{
    public float Damage = 2f;
    //public ParticleSystem ParticleSystem;

    private bool IsGeyser = false;

    private void Update()
    {
       
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Instantiate(ParticleSystem, transform.position, transform.rotation);
            if (!IsGeyser)
            {
                StartCoroutine(Geyser());
            }
        }
    }

    //private void OnTriggerExit (Collider other)
    //{
    //    if(other.CompareTag("Player"))
    //    {
    //        if(IsGeyser)
    //        {
    //            IsGeyser = true;
    //        }
    //    }
    //}
   

    IEnumerator Geyser()
    {
        IsGeyser = true;
       // Instantiate(ParticleSystem, transform.position, transform.rotation);
        GameManager.instance.playerController.TakeDamage(Damage);
        //ParticleSystem.Play(true);
        yield return new WaitForSeconds(5f);
        
        IsGeyser = false;
    }
}
