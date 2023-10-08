using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class landingZone : MonoBehaviour
{
    [SerializeField] float destroyTime;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
