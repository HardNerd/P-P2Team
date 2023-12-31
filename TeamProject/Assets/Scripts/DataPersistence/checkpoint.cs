using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class checkpoint : MonoBehaviour, IDataPersistence
{
    [SerializeField] protected Color color = new Color(255, 215, 0);
    [SerializeField] protected string guid;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        guid = System.Guid.NewGuid().ToString();
    }
    protected bool colorChanged;

    virtual protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.instance.playerSpawnPOS.transform.position != transform.position)
        {
            GameManager.instance.playerSpawnPOS.transform.position = transform.position;
            StartCoroutine(GameManager.instance.checkpointPopup());
            gameObject.GetComponentInChildren<Renderer>().material.color = color;
            colorChanged = true;
            foreach (GunStats gunStats in GameManager.instance.playerGunScript.GunList)
            {
                gunStats.savedAmmoCheckPoint = gunStats.loadedAmmo;
                gunStats.savedMaxAmmoCarriedCheckPoint = gunStats.ammoCarried;
            }
            DataPersistenceManager.Instance.SaveGame();
        }
    }

    public virtual void LoadData(GameData data)
    {
        data.checkPointColorChange.TryGetValue(guid, out colorChanged);
        if (colorChanged)
        {
            gameObject.GetComponentInChildren<Renderer>().material.color = color;
        }
    }

    public virtual void SaveData(GameData data)
    {
        if (data.checkPointColorChange.ContainsKey(guid))
        {
            data.checkPointColorChange.Remove(guid);
        }
        data.checkPointColorChange.Add(guid, colorChanged);
    }
}
