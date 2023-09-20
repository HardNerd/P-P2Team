using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endLevelGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(GameManager.instance.youWin());
        }
    }
}
