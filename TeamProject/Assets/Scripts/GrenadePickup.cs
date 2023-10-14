using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class GrenadePickup : MonoBehaviour, IDataPersistence
{
    [SerializeField] int AddThrowsAmount;
    [SerializeField] private string guid;

    private bool hasBeenCollected;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        guid = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerGrenadeGM.addThrowsMax(AddThrowsAmount);

            hasBeenCollected = true;

            Destroy(gameObject);
        }
    }

    public void LoadData(GameData data)
    {
        data.grenadePickedUp.TryGetValue(guid, out hasBeenCollected);
        if (hasBeenCollected == true)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.grenadePickedUp.ContainsKey(guid))
        {
            data.grenadePickedUp.Remove(guid);
        }
        data.grenadePickedUp.Add(guid, hasBeenCollected);
    }
}
