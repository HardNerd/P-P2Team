using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemySapwner_Heavy;
    [SerializeField] GameObject enemySapwner_Marksmen;
    [SerializeField] GameObject enemySapwner_Basic;
    [SerializeField] GameObject enemySapwner_BabyZombie;

    [SerializeField]
    private float spawenerInterval_Heavy = 3.5f;
    [SerializeField]
    private float spawenerInterval_Marksmen = 3.5f;
    [SerializeField]
    private float spawenerInterval_Basic = 3.5f;
    [SerializeField]
    private float spawenerInterval_BabyZombie = 3.5f;
    void Start()
    {
        StartCoroutine(spawnEnemy(spawenerInterval_Heavy, enemySapwner_Heavy));
        StartCoroutine(spawnEnemy(spawenerInterval_Marksmen, enemySapwner_Marksmen));
        StartCoroutine(spawnEnemy(spawenerInterval_Basic, enemySapwner_Basic));
        StartCoroutine(spawnEnemy(spawenerInterval_BabyZombie, enemySapwner_BabyZombie));
    }

    
    public IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5f), Random.Range(-6f, 6f),0), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));


    }


}
