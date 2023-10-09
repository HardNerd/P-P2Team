using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint1 : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        playerController player = other.GetComponent<playerController>();
        if (player != null)
        {
            GameManager.instance.isExiting(true);
            GameManager.instance.updatGameGoal(0);
            SceneManager.LoadScene("LevelThree");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        playerController player = other.GetComponent<playerController>();
        if (player != null)
        {
            GameManager.instance.isExiting(false);
            GameManager.instance.updatGameGoal(0);
        }
    }
}
