using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.instance.playerSpawnPOS.transform.position != transform.position)
        {
            GameManager.instance.playerSpawnPOS.transform.position = transform.position;
            StartCoroutine(GameManager.instance.checkpointPopup());
            DataPersistenceManager.Instance.SaveGame();
        }
    }
}
