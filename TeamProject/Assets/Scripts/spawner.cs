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
    [SerializeField] GameObject[] doors;
    [SerializeField] AudioSource spawnStart;
    [SerializeField] AudioSource spawnEnd;

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

    void Update()
    {
        if (startSpawning && spawnCount < maxEnemies)
        {
            foreach (GameObject door in doors)
                door.SetActive(true);
            StartCoroutine(spawn());
        }
        else if (spawnCount == maxEnemies && GameManager.instance.enemiesalive == 0)
        {
            foreach (GameObject door in doors)
                door.SetActive(false);
            if(!hasBeenSpawned)
            {
                GameManager.instance.PlaySound(spawnEnd);
            }
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
            if(!hasBeenSpawned)
            {
                GameManager.instance.PlaySound(spawnStart);
            }
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
