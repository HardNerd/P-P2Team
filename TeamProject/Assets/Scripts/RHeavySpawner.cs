using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RHeavySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemySapwner_rHeavy;




    [SerializeField] float spawenerInterval_rHeavy = 5f;

    void Start()
    {


        StartCoroutine(spawnEnemy(spawenerInterval_rHeavy, enemySapwner_rHeavy));

    }


    public IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5f), Random.Range(-6f, 6f), 0), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));


    }

}
