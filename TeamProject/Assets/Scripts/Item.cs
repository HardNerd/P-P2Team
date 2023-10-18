using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IDataPersistence
{
    public ItemObjects item;
    public string itemName;

    public void LoadData(GameData data)
    {
        data.powerUpInv.TryGetValue(itemName, out item.isCollected);
        if (item.isCollected == true)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.grenadePickedUp.ContainsKey(itemName))
        {
            data.grenadePickedUp.Remove(itemName);
        }
        data.grenadePickedUp.Add(itemName, item.isCollected);
    }
}
