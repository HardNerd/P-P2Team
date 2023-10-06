using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dash Power-UP Object", menuName = "Inventory System/Items/Dash")]

public class DashPowerUP : ItemObjects
{
    public void Awake()
    {
        type = ItemType.Power_UP;
    }
}
