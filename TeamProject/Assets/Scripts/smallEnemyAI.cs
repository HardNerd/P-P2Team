using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smallEnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;

    [Range(1, 5)] [SerializeField] int HP = 2;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void takeDamage(int damageAmount)
    {
        HP -= damageAmount;
        StartCoroutine(hitFlash());

        if (HP <= 0)
            Destroy(gameObject);
    }

    IEnumerator hitFlash()
    {
        Color origColor = model.material.color;

        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = origColor;
    }
}
