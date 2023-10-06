using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Jump Power-UP Object", menuName = "Inventory System/Items/Jump")]

public class JumpPowerUP : ItemObjects
{
    public void Awake()
    {
        type = ItemType.Power_UP;
    }
}
