using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public int deathCount;

    public int grenadeCount;

    public int grenadeToSaveBetweenLevels;

    public Vector3 playerPos;

    public int levelCount;

    public SerializableDictionary<string, bool> grenadePickedUp;

    public SerializableDictionary<string, bool> spawnersAliveData;

    public SerializableDictionary<string, bool> gunsCollected;

    public SerializableDictionary<string, bool> checkPointColorChange;
    //the values defined in this contructor will be the default values, no touch
    // the game starts with when there's no data to load
    public GameData()
    {
        this.deathCount = 0;

        this.grenadeCount = 0;
        
        this.grenadeToSaveBetweenLevels = 0;

        playerPos.x = -4.25f;
        playerPos.y = 0.5f;
        playerPos.z = -6.375f;

        this.levelCount = 0;

        grenadePickedUp = new SerializableDictionary<string, bool>();

        spawnersAliveData = new SerializableDictionary<string, bool>();

        gunsCollected = new SerializableDictionary<string, bool>();

        checkPointColorChange = new SerializableDictionary<string, bool>();
    }
}
