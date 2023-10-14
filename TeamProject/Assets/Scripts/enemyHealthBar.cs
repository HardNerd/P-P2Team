using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera playerCamera;
    [SerializeField] bool isBoss;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    private void Start()
    {
        playerCamera = GameManager.instance.mainCamera;
    }

    void Update()
    {
        if (!isBoss)
            transform.rotation = playerCamera.transform.rotation;
    }
}
