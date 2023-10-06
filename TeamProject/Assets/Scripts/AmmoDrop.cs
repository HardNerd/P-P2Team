using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AmmoDrop : MonoBehaviour
{
    public GameObject ammoDropPrefab;

    public void ammoDrop()
    {
        Instantiate(ammoDropPrefab, transform.position,Quaternion.identity);
    }

}
