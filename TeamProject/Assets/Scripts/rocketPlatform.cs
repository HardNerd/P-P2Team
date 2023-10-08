using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketPlatform : MonoBehaviour
{
    public bool playerInside;
    public Transform target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }
}
