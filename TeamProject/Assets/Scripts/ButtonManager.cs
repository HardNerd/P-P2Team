using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonManager : MonoBehaviour
{
    [SerializeField] AudioSource clickNoise;
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    // Start is called before the first frame update
    public void begin()
    {
       // DataPersistenceManager.Instance.NewGame();
        clickNoise.Play();
        StartCoroutine(beginTime());
    }

    public IEnumerator beginTime()
    {
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadSceneAsync("Level One");
        Time.timeScale = 1;
        GameManager.instance.stateUnpause();
    }
    public void OnContinueClicked()
    {
        Debug.Log("Continue Game Clicked");
        SceneManager.LoadSceneAsync("Level One");
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
        GameManager.instance.levelTime.timeTaken = GameManager.instance.levelTime.timeBuff;
        //Include respawn here
    }

    public void quit()
    {
        clickNoise.Play();
        Application.Quit();
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


}
