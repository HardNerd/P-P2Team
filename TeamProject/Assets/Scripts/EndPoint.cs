using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour, IDataPersistence
{
    [SerializeField] Vector3 vector3 = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] string nextSceneLoaded;
    public int throwsToSave;
    public void LoadData(GameData data)
    {
        throwsToSave = data.grenadeToSaveBetweenLevels;
    }

    public void SaveData(GameData data)
    {
        data.levelCount = GameManager.instance.levelClearedAmount;
        data.grenadeToSaveBetweenLevels = throwsToSave;
    }

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        
        playerController player = other.GetComponent<playerController>();
        if (player != null)
        {
            GameManager.instance.isExiting(true);
            GameManager.instance.updatGameGoal(0);
            GameManager.instance.playerSpawnPOS.transform.position = vector3;
            GameManager.instance.levelClearedAmount++;
            GameManager.instance.bossesKilledGM++;
            foreach (GunStats gunStats in GameManager.instance.playerGunScript.GunList)
            {
                gunStats.savedAmmoNextLvl = gunStats.loadedAmmo;
                gunStats.savedMaxAmmoNextLvl = gunStats.ammoCarried;
            }
            throwsToSave = GameManager.instance.playerGrenadeGM.totalThrows;
            DataPersistenceManager.Instance.SaveGame();
            SceneManager.LoadSceneAsync(nextSceneLoaded);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        playerController player = other.GetComponent<playerController>();
        if (player != null)
        {
            GameManager.instance.isExiting(false);
            GameManager.instance.updatGameGoal(0);
        }
    }
}
