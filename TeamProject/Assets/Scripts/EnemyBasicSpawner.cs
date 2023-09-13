using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicSpawner : MonoBehaviour
{
    [SerializeField] GameObject enemySapwner_Basic;




    [SerializeField] float spawenerInterval_Basic = 3.5f;

    void Start()
    {


        StartCoroutine(spawnEnemy(spawenerInterval_Basic, enemySapwner_Basic));

    }


    public IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5f), Random.Range(-6f, 6f), 0), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));


    }

}
