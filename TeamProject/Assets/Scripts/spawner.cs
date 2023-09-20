using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemiesToSpawn;
    [SerializeField] int maxEnemies;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] int timeOffset;

    bool isSpawning;
    bool startSpawning;
    int spawnCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(startSpawning && spawnCount < maxEnemies)
        {
            StartCoroutine(spawn());
        }
    }

    public IEnumerator spawn()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            int arraySpawnPos = Random.Range(0, spawnPos.Length);
            int indexer = Random.Range(0, enemiesToSpawn.Length);
            GameObject enemySpawned = Instantiate(enemiesToSpawn[indexer], spawnPos[arraySpawnPos].position, spawnPos[arraySpawnPos].rotation);

            spawnCount++;

            yield return new WaitForSeconds(timeOffset);
            isSpawning=false;
        }
    }

    public void enemyDeath()
    {
        maxEnemies--;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }
}
