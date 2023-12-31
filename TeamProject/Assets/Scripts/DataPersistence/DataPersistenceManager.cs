using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Runtime.ConstrainedExecution;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]

    [SerializeField] private bool initializeDataIfNull = false;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler dataHandler;

    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        OverwriteCurrSaveData();
    }

    public void RestartLvl1()
    {
        GameManager.instance.inventoryObjectsGM.ClearItems();

        GameManager.instance.bossesKilledGM = 0;

        this.gameData.playerPos.x = -4.25f;
        this.gameData.playerPos.y = 0.5f;
        this.gameData.playerPos.z = -6.375f;

        this.gameData.gunsCollected = null;

        this.gameData.spawnersAliveData = null;

        this.gameData.checkPointColorChange = null;

        this.gameData.grenadeCount = 0;

        this.gameData.grenadePickedUp = null;   

        OverwriteCurrSaveData();
    }

    public void RestartLvl2()
    {
        GameManager.instance.inventoryObjectsGM.ClearOnlyLastTwoItems();

        GameManager.instance.bossesKilledGM = 2;

        this.gameData.playerPos.x = -4;
        this.gameData.playerPos.y = 1;
        this.gameData.playerPos.z = -0.25f;

        this.gameData.grenadeCount = this.gameData.grenadeToSaveBetweenLevels;

        string assultGuid = "3e69fa03-63ea-4ce8-a426-7b60d13c0bfe";

        if (this.gameData.gunsCollected.ContainsKey(assultGuid))
        {
            this.gameData.gunsCollected.Remove(assultGuid);
            this.gameData.gunsCollected.Add(assultGuid, false);
        }
        string spawner1 = "b1b25e5c-cddf-4637-b41b-f00dcdcec2db";
        string spawner2 = "d56bbaee-831e-4966-a14b-8b11cba66859";

        this.gameData.spawnersAliveData.Remove(spawner1);
        this.gameData.spawnersAliveData.Remove(spawner2);
        this.gameData.spawnersAliveData.Add(spawner1, false);
        this.gameData.spawnersAliveData.Add(spawner2, false);

        string checkpoint1 = "ec9512e3-a2fb-49a5-9c35-c46e2999daa4";
        string checkpoint2 = "4fa8fced-8a37-47e2-8c37-48dbe59dd2b3";
        string checkpoint3 = "fd9c48e6-11c9-40c4-a1e4-b6106a105586";
        string checkpoint4 = "b046d0ae-a6b1-4fa0-8148-bad03ba9f221";
        string checkpoint5 = "811593ec-d5d0-4941-9c32-a06b04be230c";

        this.gameData.checkPointColorChange.Remove(checkpoint1);
        this.gameData.checkPointColorChange.Remove(checkpoint2);
        this.gameData.checkPointColorChange.Remove(checkpoint3);
        this.gameData.checkPointColorChange.Remove(checkpoint4);
        this.gameData.checkPointColorChange.Remove(checkpoint5);
        this.gameData.checkPointColorChange.Add(checkpoint1, false);
        this.gameData.checkPointColorChange.Add(checkpoint2, false);
        this.gameData.checkPointColorChange.Add(checkpoint3, false);
        this.gameData.checkPointColorChange.Add(checkpoint4, false);
        this.gameData.checkPointColorChange.Add(checkpoint5, false);

        string grenadePickUp1 = "20f3defa-1f8b-4abd-83ff-988b670c98f1";
        string grenadePickUp2 = "3b21e085-b94a-4e9c-99af-083b473a4a42";
        string grenadePickUp3 = "2c6980d7-cc35-4459-9481-f5f4b354713a";

        this.gameData.grenadePickedUp.Remove(grenadePickUp1);
        this.gameData.grenadePickedUp.Remove(grenadePickUp2);
        this.gameData.grenadePickedUp.Remove(grenadePickUp3);
        this.gameData.grenadePickedUp.Add(grenadePickUp1, false);
        this.gameData.grenadePickedUp.Add(grenadePickUp2, false);
        this.gameData.grenadePickedUp.Add(grenadePickUp3, false);

        OverwriteCurrSaveData();
    }
    public void RestartLvl3()
    {
        GameManager.instance.bossesKilledGM = 4;

        this.gameData.playerPos.x = -3;
        this.gameData.playerPos.y = 1;
        this.gameData.playerPos.z = 3;

        this.gameData.grenadeCount = this.gameData.grenadeToSaveBetweenLevels;

        string uziGuid = "7fa89e7a-65f8-4d3f-b5e6-011bdd3b240f";

        if (this.gameData.gunsCollected.ContainsKey(uziGuid))
        {
            this.gameData.gunsCollected.Remove(uziGuid);
            this.gameData.gunsCollected.Add(uziGuid, false);
        }
        
        string spawner1 = "8d22830a-44a1-4d49-a8cc-8e8783d4254b";
        string spawner2 = "75f223ca-b06d-4acd-8e28-daced79b20e4";
        string spawner3 = "e527e157-af4b-40e1-9b68-9d80d8908cf2";
        string spawner4 = "ac6b6379-4d78-4800-9ebc-ad3a215ba958";
        string spawner5 = "cb97d14b-20ad-4624-b427-b3f6770c58f1";


        this.gameData.spawnersAliveData.Remove(spawner1);
        this.gameData.spawnersAliveData.Remove(spawner2);
        this.gameData.spawnersAliveData.Remove(spawner3);
        this.gameData.spawnersAliveData.Remove(spawner4);
        this.gameData.spawnersAliveData.Remove(spawner5);
        this.gameData.spawnersAliveData.Add(spawner1, false);
        this.gameData.spawnersAliveData.Add(spawner2, false);
        this.gameData.spawnersAliveData.Add(spawner3, false);
        this.gameData.spawnersAliveData.Add(spawner4, false);
        this.gameData.spawnersAliveData.Add(spawner5, false);

        string checkpoint1 = "d7bf76e8-0486-418b-9667-30f8f17dfe25";
        string checkpoint2 = "05ffdebf-dbaf-4bee-9278-165d8df29047";
        string checkpoint3 = "4a67b431-57b5-4187-b03d-8d34b7f36aae";
        string checkpoint4 = "b3f4eae8-2375-45c1-be6a-777f53269609";

        this.gameData.checkPointColorChange.Remove(checkpoint1);
        this.gameData.checkPointColorChange.Remove(checkpoint2);
        this.gameData.checkPointColorChange.Remove(checkpoint3);
        this.gameData.checkPointColorChange.Remove(checkpoint4);
        this.gameData.checkPointColorChange.Add(checkpoint1, false);
        this.gameData.checkPointColorChange.Add(checkpoint2, false);
        this.gameData.checkPointColorChange.Add(checkpoint3, false);
        this.gameData.checkPointColorChange.Add(checkpoint4, false);

        string grenadePickUp = "b755600a-3e7e-45d6-9434-f3892c449125";
        string grenadePickUp2 = "5df5c824-5263-4079-8a2d-681a33ddc482";

        this.gameData.grenadePickedUp.Remove(grenadePickUp);
        this.gameData.grenadePickedUp.Remove(grenadePickUp2);
        this.gameData.grenadePickedUp.Add(grenadePickUp, false);
        this.gameData.grenadePickedUp.Add(grenadePickUp2, false);
        OverwriteCurrSaveData();
    }

    public void LoadGame()
    {
        // Load any save data from a file unsing the data handler
        this.gameData = dataHandler.Load();
        //if no data can be loaded, initialize to a new game
        if (this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }
        //push loaded data to all other scripts that need it

        if (this.gameData == null)
        {
            return;
        }

        foreach (IDataPersistence dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.LoadData(gameData);
        }
    }
    public void OverwriteCurrSaveData()
    {
        dataHandler.Save(gameData);
    }
    public void SaveGame()
    {

        // pass data to other scripts that can interact with it
        foreach (IDataPersistence dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.SaveData(gameData);
        }

        // save the data to a file using the data handler
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    public bool HasGameData()
    {
        return this.gameData != null;
    }
}
