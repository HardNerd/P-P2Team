using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
//using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]

public class InventoryObjects : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>(); // always 4

    public void AddItem(ItemObjects _item)
    {
        //bool hasItem = false;
        //for(int i = 0; i < Container.Count; i++)
        //{
        //    if (Container[i].Item == _item)
        //    {
        //        //Container[i].AddAmmount(_amount);
        //        hasItem = true;
        //        break;
        //    }
        //}
        //Container.Add(new InventorySlot(_item));

        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].Item == null)
            {
                Container[i].Add(_item);
                return;
            }
        }
    }

    public void ClearItems()
    {
        for (int i = 0; i < Container.Count; i++)
            Container[i].Clear();
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemObjects Item;
    //public int amount;
    public InventorySlot(ItemObjects _item)
    {
        Item = _item;
        //amount = _amount;
    }

    public void Add(ItemObjects _item)
    {
        Item = _item;
    }

    public void Clear()
    {
        Item = null;
    }

    //public void AddAmmount(int value)
    //{
    //    amount += value;
    //}
}
