using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [SerializeField] int explosionSize;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        IPhysics isphysicpossible = other.GetComponent<IPhysics>();

        if (isphysicpossible != null)
        {
            isphysicpossible.physics((other.transform.position - transform.position).normalized * explosionSize);
        }
    }
}
