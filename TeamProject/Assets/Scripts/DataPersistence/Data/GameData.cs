using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public int deathCount;

    public Vector3 playerPos;

    public SerializableDictionary<string, bool> spawnersAliveData;

    public SerializableDictionary<string, bool> gunsCollected;
    //the values defined in this contructor will be the default values, no touch
    // the game starts with when there's no data to load
    public GameData()
    {
        this.deathCount = 0;
        playerPos = Vector3.zero;
        spawnersAliveData = new SerializableDictionary<string, bool>();
        gunsCollected = new SerializableDictionary<string, bool>();
    }
}
