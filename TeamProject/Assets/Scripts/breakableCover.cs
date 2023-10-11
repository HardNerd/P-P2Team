using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakableCover : MonoBehaviour, IDamage
{
    [SerializeField] int breakPoints;
    [SerializeField] Renderer model;

    public void TakeDamage(float damageAmount, string source = null)
    {
        breakPoints--;
        StartCoroutine(FlashDamage());

        if (breakPoints <= 0)
            Destroy(gameObject);
    }
    IEnumerator FlashDamage()
    {
        Color origColor = model.material.color;
        model.material.color = Color.gray;
        yield return new WaitForSeconds(0.1f);
        model.material.color = origColor;
    }
}
