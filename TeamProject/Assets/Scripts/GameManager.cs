using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;
    public playerController playerController;

    [SerializeField] GameObject activeMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] GameObject playUI;

    //I expect the player person to set this up I'm just putting it here for later
    public GameObject playerSpawnPOS;

    bool isPause;
    float currtime;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<playerController>();
        currtime = Time.timeScale;
    }
    void Update()
    {
        if(Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            statePause();
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPause);
        }
    }

    public void statePause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        isPause = !isPause;
        playUI.SetActive(!isPause);
    }

    public void stateUnpause()
    {
        Time.timeScale = currtime;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPause = !isPause;
        playUI.SetActive(!isPause);
        activeMenu.SetActive(isPause);
        activeMenu = null;
    }

    public void loseScreen()
    {
        statePause();
        activeMenu = loseMenu;
        activeMenu.SetActive(isPause);

    }
}
