using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] AudioSource clickNoise;
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] public string loadScene1 = "Level One";
    [SerializeField] public string loadScene2 = "LevelTwo";
    [SerializeField] public string loadScene3 = "LevelThree";


    private void Start()
    {
        if (!DataPersistenceManager.Instance.HasGameData())
        {
            continueGameButton.interactable = false;
        }
    }

    public void begin()
    {
        clickNoise.Play();
        if (DataPersistenceManager.Instance.HasGameData())
            GameManager.instance.SaveOverlay(true);
        else
        {
            DataPersistenceManager.Instance.NewGame();
        }
    }

    public void no()
    {
        clickNoise.Play();
        GameManager.instance.SaveOverlay(false);
    }

    public void yes()
    {
        clickNoise.Play();
        if (SceneManager.GetActiveScene().name == "KevinScene")
        {
            DataPersistenceManager.Instance.NewGame();
            GameManager.instance.SaveOverlay(false);
            StartCoroutine(beginTime());
        }
    }

    public IEnumerator beginTime()
    {
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadSceneAsync(loadScene1);
        Time.timeScale = 1;
        GameManager.instance.stateUnpause();
    }

    public void OnContinueClicked()
    {
        clickNoise.Play();
        Debug.Log("Continue Game Clicked");
        DataPersistenceManager.Instance.LoadGame();
        StartCoroutine(ContinueGameTime());
    }

    public IEnumerator ContinueGameTime()
    {
        yield return new WaitForSecondsRealtime(1);
        if (DataPersistenceManager.Instance != null)
        {
            if (GameManager.instance.levelClearedAmount == 2)
            {
                SceneManager.LoadSceneAsync(loadScene3);
                Time.timeScale = 1;
                GameManager.instance.stateUnpause();
            }
            else if (GameManager.instance.levelClearedAmount == 1)
            {
                SceneManager.LoadSceneAsync(loadScene2);
                Time.timeScale = 1;
                GameManager.instance.stateUnpause();
            }
            else
            {
                SceneManager.LoadSceneAsync(loadScene1);
                Time.timeScale = 1;
                GameManager.instance.stateUnpause();
            }
        }
    }

    public void unpause()
    {
        clickNoise.Play();
        GameManager.instance.stateUnpause();
    }

    public void restart()
    {
        clickNoise.Play();
        StartCoroutine(restartTime());
    }

    public IEnumerator restartTime()
    {
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.stateUnpause();
    }

    public void respawn()
    {
        clickNoise.Play();
        GameManager.instance.stateUnpause();
        
        GameManager.instance.playerController.spawnPlayer();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        DataPersistenceManager.Instance.LoadGame();
        //GameManager.instance.levelTime.timeTaken = GameManager.instance.levelTime.timeBuff;
        //Include respawn here
    }

    public void quit()
    {
        clickNoise.Play();
        if (SceneManager.GetActiveScene().name == "KevinScene")
            Application.Quit();
        else
            StartCoroutine(titleLoad());
    }

    public IEnumerator titleLoad()
    {
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene("KevinScene");
    }

    public void options()
    {
        clickNoise.Play();
        GameManager.instance.options();
    }

    public void back()
    {
        clickNoise.Play();
        GameManager.instance.back();
    }

    public void credits()
    {
        clickNoise.Play();
        GameManager.instance.credits();
    }

    public void LoadData(GameData data)
    {
        
    }

    public void SaveData(GameData data)
    {

    }
}
