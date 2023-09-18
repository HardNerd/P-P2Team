using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GHeavySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemySapwner_gHeavy;




    [SerializeField] float spawenerInterval_gHeavy = 5f;

    void Start()
    {


        StartCoroutine(spawnEnemy(spawenerInterval_gHeavy, enemySapwner_gHeavy));

    }


    public IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5f), Random.Range(-6f, 6f), 0), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));


    }

}
