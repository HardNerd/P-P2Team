using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour, IDataPersistence
{
    [SerializeField] Vector3 vector3 = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] string nextSceneLoaded;

    [SerializeField] private string guid;
    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        guid = System.Guid.NewGuid().ToString();
    }

    public void LoadData(GameData data)
    {
        data.levelsCleared.TryGetValue(guid, out GameManager.instance.levelCleared);
        if (GameManager.instance.levelCleared == true )
        {
            SceneManager.LoadSceneAsync(nextSceneLoaded);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.levelsCleared.ContainsKey(guid))
        {
            data.levelsCleared.Remove(guid);
        }
        data.levelsCleared.Add(guid, GameManager.instance.levelCleared);
    }

    

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        
        playerController player = other.GetComponent<playerController>();
        if (player != null)
        {
            GameManager.instance.isExiting(true);
            GameManager.instance.updatGameGoal(0);
            GameManager.instance.playerSpawnPOS.transform.position = vector3;
            GameManager.instance.levelCleared = true;
            DataPersistenceManager.Instance.SaveGame();
            SceneManager.LoadSceneAsync(nextSceneLoaded);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        playerController player = other.GetComponent<playerController>();
        if (player != null)
        {
            GameManager.instance.isExiting(false);
            GameManager.instance.updatGameGoal(0);
        }
    }
}
