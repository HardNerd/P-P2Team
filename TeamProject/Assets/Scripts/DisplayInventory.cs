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

    // Start is called before the first frame update
    void Start()
    {
        //CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
       //UpdateDisplay();
    }

    //public void UpdateDisplay()
    //{
    //    for(int i = 0; i < inventory.Container.Count; i++)
    //    {
    //        //if (itemsDisplay.ContainsKey(inventory.Container[i]))
    //        //{
    //        //    itemsDisplay[inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
    //        //}
    //        var objects = Instantiate(inventory.Container[i].Item.item, Vector3.zero, Quaternion.identity, transform);
    //        objects.GetComponent<RectTransform>().localPosition = GetPosition(i);
    //        //objects.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
    //        itemsDisplay.Add(inventory.Container[i], objects);
    //    }
    //}

    public void UpdateDisplay()
    {
        for(int i = 0; i < inventory.Container.Count; i++)
        {
            var imageObject = Instantiate(inventory.Container[i].Item.image, Vector3.zero, Quaternion.identity, transform);
            imageObject.GetComponent<RectTransform>().localPosition = GetPosition(i);
            //objects.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
            itemsDisplay.Add(inventory.Container[i], imageObject);
        }
    }

    public void DisplayItem()
    {
        var imageObject = Instantiate(inventory.Container[index].Item.image, Vector3.zero, Quaternion.identity, transform);
        imageObject.GetComponent<RectTransform>().localPosition = GetPosition(index);
        //objects.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
        itemsDisplay.Add(inventory.Container[index], imageObject);
        index++;
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + (X_Space_Betwwen_Items * (i % Number_Of_Rows)), yStart + (-Y_Space_Betwwen_Items * (i % Number_Of_Rows)), 0f);
    }
}
