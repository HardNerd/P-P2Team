using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]

public class InventoryObjects : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>(); 

    public void AddItem(ItemObjects _item, int _amount)
    {
        bool hasItem = false;
        for(int i = 0; i < Container.Count; i++)
        {
            if (Container[i].Item == _item)
            {
                Container[i].AddAmmount(_amount);
                hasItem = true;
                break;
            }
        }
        if (hasItem)
        {
            Container.Add(new InventorySlot(_item, _amount));
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemObjects Item;
    public int amount;
    public InventorySlot(ItemObjects _item, int _amount)
    {
        Item = _item;
        amount = _amount;
    }

    public void AddAmmount(int value)
    {
        amount += value;
    }
}
