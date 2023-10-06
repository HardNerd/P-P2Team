using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Double Jump Power-UP Object", menuName = "Inventory System/Items/Double Jump")]

public class DoubleJumpPowerUP : ItemObjects
{
    public void Awake()
    {
        type = ItemType.Power_UP;
    }
}
