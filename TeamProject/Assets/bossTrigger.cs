using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossTrigger : MonoBehaviour
{
    [SerializeField] GameObject boss;

    private void Start()
    {
        boss.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boss.SetActive(true);
            GameManager.instance.playerSpawnPOS.transform.position = transform.position;
        }
    }
}
