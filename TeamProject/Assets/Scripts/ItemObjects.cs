using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Power_UP
}

public abstract class ItemObjects : ScriptableObject
{
    public bool isCollected;
    public GameObject image;
    public ItemType type;
    [TextArea(15,20)]
    public string description;
}
