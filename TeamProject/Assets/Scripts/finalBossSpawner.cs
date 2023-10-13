using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finalBossSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemiesToSpawn;
    [SerializeField] int maxEnemies;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] int timeOffset;

    bool isSpawning;
    bool hasBeenSpawned;
    public bool startSpawning;
    int spawnCount;

    void Update()
    {
        if (startSpawning && spawnCount < maxEnemies)
        {
            StartCoroutine(spawn());
        }
        else if (spawnCount == maxEnemies && GameManager.instance.enemiesalive == 0)
        {
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
}
