using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class BossCheckPoint : checkpoint, IDataPersistence
{
    [SerializeField] private int thisBossNumber;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (GameManager.instance.bossesKilledGM < thisBossNumber)
        {
            GameManager.instance.bossesKilledGM++;
        }
    }
    public override void LoadData(GameData data)
    {
        GameManager.instance.bossesKilledGM = data.bossesKilled;
    }
    public override void SaveData(GameData data)
    {
        data.bossesKilled = GameManager.instance.bossesKilledGM;
    }
}
