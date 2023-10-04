using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;



public class DisappearingPlatform : MonoBehaviour
{
    float disapearInterval = 5.0f;
    public GameObject platform;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Disapear());
    }

    IEnumerator Disapear()
    {
       
        while (true)
        {
            platform.SetActive(true);
            yield return new WaitForSeconds(disapearInterval / 2f);
            platform.SetActive(false);
            yield return new WaitForSeconds(disapearInterval / 2f);
        }

       

    }
}
