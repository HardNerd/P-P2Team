using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour, IDataPersistence
{
    [SerializeField] Vector3 vector3 = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] string nextSceneLoaded;

    [SerializeField] private string levelCleared;
    public void LoadData(GameData data)
    {
      
    }

    public void SaveData(GameData data)
    {
        data.levelCount = GameManager.instance.levelClearedAmount;
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
            GameManager.instance.levelClearedAmount++;
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
