using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBabyZombieSpawner : MonoBehaviour
{
    [SerializeField] GameObject enemySapwner_BabyZombie;


    [SerializeField] float spawenerInterval_BabyZombie = 3.5f;

    void Start()
    {
        StartCoroutine(spawnEnemy(spawenerInterval_BabyZombie, enemySapwner_BabyZombie));
    }

    public IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5f), Random.Range(-6f, 6f), 0), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
    }
}
