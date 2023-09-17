using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class pickupScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.updatGameGoal(1, false);
    }

    public void OnTriggerEnter(Collider other)
    {
        playerController pc = other.GetComponent<playerController>();

        if (pc != null)
        {
            GameManager.instance.updatGameGoal(-1, false);
            Destroy(gameObject);
        }
    }
}
