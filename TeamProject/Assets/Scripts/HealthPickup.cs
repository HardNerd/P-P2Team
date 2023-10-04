using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    
    public AudioClip healthPickupSound;
    public GameObject useText;
    public static bool hasPickedUpHealthPack = false;
    private bool hasEneteredTrigger = false;

    private void Update()
    {
        if (hasEneteredTrigger)
        {

            if (Input.GetButton("Interact"))
            {
                hasPickedUpHealthPack = true;
                hasEneteredTrigger = false;
                gameObject.SetActive(false);
                useText.SetActive(false);
            }
            GameManager.instance.playerController.healthPickup();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        useText.SetActive(true);
        hasEneteredTrigger = true;


    }

    private void OnTriggerExit(Collider other)
    {
        useText.SetActive(false);
        hasPickedUpHealthPack = false;
        hasEneteredTrigger = false;
    }



}
