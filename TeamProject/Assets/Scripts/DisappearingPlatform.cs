using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;



public class DisappearingPlatform : MonoBehaviour
{
    float disappearInterval = 5.0f;
    float reappearInterval = 5.0f;
    public GameObject platform;



    private void OnTriggerEnter(Collider other)
    {

        StartCoroutine(Disapear());
        
    }

    void OnTriggerExit(Collider other)
    {
        StartCoroutine(Reappear());
    }

    IEnumerator Disapear()
    {
        yield return new WaitForSeconds(disappearInterval);
        platform.SetActive(false);
    }

    IEnumerator Reappear()
    {
        yield return new WaitForSeconds(reappearInterval);
        platform.SetActive(true);
    }

    


}
