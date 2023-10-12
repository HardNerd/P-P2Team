using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

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
        NewSaveGame();
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
    public void NewSaveGame()
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
