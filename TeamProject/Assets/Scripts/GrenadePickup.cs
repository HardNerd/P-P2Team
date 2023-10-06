using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePickup : MonoBehaviour
{
    [SerializeField] int AddThrowsAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerGrenadeGM.addThrowsMax(AddThrowsAmount);
            Destroy(gameObject);
        }
    }
}
