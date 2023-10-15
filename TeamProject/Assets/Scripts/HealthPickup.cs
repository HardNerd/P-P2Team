using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    
    public AudioClip healthPickupSound;
    public GameObject useTextPrefab;
    public GameObject useText;
    public static bool hasPickedUpHealthPack = false;
    private bool hasEneteredTrigger = false;

    bool isInstantiated;

    private void Update()
    {
        if (hasEneteredTrigger)
        {

            if (Input.GetButton("Interact"))
            {
                hasPickedUpHealthPack = true;
                hasEneteredTrigger = false;
                gameObject.SetActive(false);
                useTextPrefab.SetActive(false);
            }
            GameManager.instance.playerController.healthPickup();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isInstantiated)
        {
            isInstantiated = true;
            useText = Instantiate(useTextPrefab, Vector3.zero, Quaternion.identity, transform);
        }

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
