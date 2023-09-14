using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMarksmanSpawner : MonoBehaviour
{
    [SerializeField] GameObject enemySapwner_Marksmen;

    [SerializeField] float spawenerInterval_Marksmen = 3.5f;
    void Start()
    {
        StartCoroutine(spawnEnemy(spawenerInterval_Marksmen, enemySapwner_Marksmen));
    }

    public IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5f), Random.Range(-6f, 6f), 0), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
    }
}
