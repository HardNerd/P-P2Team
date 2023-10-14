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
            Debug.LogError("Found more than one Data Persistance Manager in the scene. Destroying the newest one.");
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
        Debug.Log("OnSceneLoaded Called");
        //gameData.playerPos = GameManager.instance.playerSpawnPOS.transform.position;
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
        this.gameData.playerPos.x = -4;
        this.gameData.playerPos.y = 1;
        this.gameData.playerPos.z = -0.25f;

        string assultGuid = "3e69fa03-63ea-4ce8-a426-7b60d13c0bfe";

        if (this.gameData.gunsCollected.ContainsKey(assultGuid))
        {
            this.gameData.gunsCollected.Remove(assultGuid);
            this.gameData.gunsCollected.Add(assultGuid, false);
        }

        string spawner1 = "9bddf22c-1cc5-49b9-92bb-f9fa03606f88";
        string spawner2 = "9154a5d0-a0eb-4720-9cff-a9c38ee4f764";
        
        this.gameData.spawnersAliveData.Remove(spawner1 + spawner2);

        string checkpoint1 = "ec9512e3-a2fb-49a5-9c35-c46e2999daa4";
        string checkpoint2 = "811593ec-d5d0-4941-9c32-a06b04be230c";

        this.gameData.checkPointColorChange.Remove(checkpoint1 + checkpoint2);

        OverwriteCurrSaveData();
    }
    public void RestartLvl3()
    {
        this.gameData.playerPos.x = -3;
        this.gameData.playerPos.y = 1;
        this.gameData.playerPos.z = 3;

        string uziGuid = "7fa89e7a-65f8-4d3f-b5e6-011bdd3b240f";

        if (this.gameData.gunsCollected.ContainsKey(uziGuid))
        {
            this.gameData.gunsCollected.Remove(uziGuid);
            this.gameData.gunsCollected.Add(uziGuid, false);
        }

        string spawner1 = "168aa64e-f3bd-468c-a064-e724b953a1c1";
        string spawner2 = "a332f6b8-28e6-4836-9ed2-cee9c1ad0014";
        string spawner3 = "eb41d453-548a-4d1c-9e3f-b9b98294be2b";
        string spawner4 = "62039fc9-33be-4bf8-b08a-95a1c54f3765";
        string spawner5 = "f9d30ba9-a6ba-4587-a7e3-7a4ec2bd085f";
        string spawner6 = "a8e58a94-025a-4203-a4a5-d0114c6d341a";

        
        this.gameData.spawnersAliveData.Remove(spawner1 + spawner2 +spawner3 + spawner4 + spawner5 + spawner6);


        string checkpoint1 = "d7bf76e8-0486-418b-9667-30f8f17dfe25";
        string checkpoint2 = "05ffdebf-dbaf-4bee-9278-165d8df29047";
        string checkpoint3 = "4a67b431-57b5-4187-b03d-8d34b7f36aae";

        this.gameData.checkPointColorChange.Remove(checkpoint1 + checkpoint2 + checkpoint3);

        OverwriteCurrSaveData();
    }

    public void LoadGame()
    {
        // Load any save data from a file unsing the data handler
        this.gameData = dataHandler.Load();
        // if no data can be loaded, initialize to a new game
        if (this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }


        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults");
            return;
        }
        //push loaded data to all other scripts that need it
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
        if (this.gameData == null)
        {
            Debug.LogWarning("No data was found. A new game needs to be started before data can be saved.");
            return;
        }

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
