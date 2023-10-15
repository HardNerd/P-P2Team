using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossTrigger : MonoBehaviour
{
    [SerializeField] GameObject boss;
    [SerializeField] GameObject[] doors;
    private void Start()
    {
        boss.SetActive(false);

        for (int i = 0; i < doors.Length; i++)
            doors[i].SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boss.SetActive(true);

            for (int i = 0; i < doors.Length; i++)
                doors[i].SetActive(true);
        }
    }
}
