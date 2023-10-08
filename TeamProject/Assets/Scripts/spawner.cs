using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class spawner : MonoBehaviour, IDataPersistence
{
    [SerializeField] GameObject[] enemiesToSpawn;
    [SerializeField] int maxEnemies;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] int timeOffset;
    [SerializeField] GameObject door1;
    [SerializeField] GameObject door2;
    [SerializeField] GameObject door3;
    [SerializeField] GameObject door4;

    bool isSpawning;
    bool hasBeenSpawned;
    public bool startSpawning;
    int spawnCount;

    [SerializeField] private string guid;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        guid = System.Guid.NewGuid().ToString();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && spawnCount < maxEnemies)
        {
            door1.SetActive(true);
            door2.SetActive(true);
            door3.SetActive(true);
            door4.SetActive(true);
            StartCoroutine(spawn());
        }
        else if (spawnCount == maxEnemies && GameManager.instance.enemiesalive == 0)
        {
            door1.SetActive(false); 
            door2.SetActive(false);
            door3.SetActive(false); 
            door4.SetActive(false);

            hasBeenSpawned = true;
            StopCoroutine(spawn());
        }
    }

    public IEnumerator spawn()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            GameManager.instance.updatGameGoal(1);
            int arraySpawnPos = Random.Range(0, spawnPos.Length);
            int indexer = Random.Range(0, enemiesToSpawn.Length);
            Instantiate(enemiesToSpawn[indexer], spawnPos[arraySpawnPos].position, spawnPos[arraySpawnPos].rotation);

            spawnCount++;

            yield return new WaitForSeconds(timeOffset);
            isSpawning = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }

    public void LoadData(GameData data)
    {
        data.spawnersAliveData.TryGetValue(guid, out hasBeenSpawned);
        if (hasBeenSpawned == true)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.spawnersAliveData.ContainsKey(guid))
        {
            data.spawnersAliveData.Remove(guid);
        }
        data.spawnersAliveData.Add(guid, hasBeenSpawned);
    }
}
