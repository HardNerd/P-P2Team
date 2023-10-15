using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObjects inventory;

    public int xStart;
    public int yStart;
    public int X_Space_Betwwen_Items;
    public int Number_Of_Rows;
    public int Y_Space_Betwwen_Items;
    Dictionary<InventorySlot,GameObject> itemsDisplay = new Dictionary<InventorySlot,GameObject>();

    int index = 0;

    public void UpdateDisplay()
    {
        for(int i = 0; i < inventory.Container.Count; i++)
        {
            var imageObject = Instantiate(inventory.Container[i].Item.image, Vector3.zero, Quaternion.identity, transform);
            imageObject.GetComponent<RectTransform>().localPosition = GetPosition(i);
            itemsDisplay.Add(inventory.Container[i], imageObject);
        }
    }

    public void DisplayItem()
    {
        var imageObject = Instantiate(inventory.Container[index].Item.image, Vector3.zero, Quaternion.identity, transform);
        imageObject.GetComponent<RectTransform>().localPosition = GetPosition(index);
        itemsDisplay.Add(inventory.Container[index], imageObject);
        index++;
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + (X_Space_Betwwen_Items * (i % Number_Of_Rows)), yStart + (-Y_Space_Betwwen_Items * (i % Number_Of_Rows)), 0f);
    }
}
