using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;



public class DisappearingPlatform : MonoBehaviour
{
    float disapearInterval = 5.0f;
    public GameObject platform;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Disapear());
    }

 
   
    

    IEnumerator Disapear()
    {
       
       
            platform.SetActive(!platform.activeSelf);
            yield return new WaitForSeconds(disapearInterval / 2f);
            platform.SetActive(platform.activeSelf);
            yield return new WaitForSeconds(disapearInterval / 2f);
        

       

    }
}
