using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void unpause()
    {
        GameManager.instance.stateUnpause();
    }

    public void begin()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.stateUnpause();
    }

    public void respawn()
    {
        GameManager.instance.stateUnpause();
        GameManager.instance.playerController.spawnPlayer();
        GameManager.instance.levelTime.timeTaken = GameManager.instance.levelTime.timeBuff;
        //Include respawn here
    }

    public void quit()
    {
        Application.Quit();
    }
}
