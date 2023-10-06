using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sprint Power-UP Object", menuName ="Inventory System/Items/Sprint")]

public class SprintPowerUP : ItemObjects
{
    public void Awake()
    {
        type = ItemType.Power_UP;
    }
}
