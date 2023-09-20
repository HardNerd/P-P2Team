using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("----- Player Data -----")]
    public GameObject player;
    public playerController playerController;
    public GameObject playerSpawnPOS;

    [Header("----- Menus -----")]
    [SerializeField] GameObject activeMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] GameObject checkpointMenu;

    [Header("----- Play State -----")]
    [SerializeField] GameObject endPoint;
    [SerializeField] GameObject playUI;
    [SerializeField] GameObject timer;
    [SerializeField] TMP_Text objectiveText;
    public levelTimer levelTime;
    public Image healthRed;
    public Image healthYel;
    public Image stamBlue;
    public Image stamYel;

    int enemiesalive;
    int pickupsLeft;

    //I expect the player person to set this up I'm just putting it here for later

    public bool isPause;
    float currtime;

    bool isInsideExit;

    public float healthRedFillAmt;
    public float healthYelFillAmt;
    public float stamBlueFillAmt;
    public float stamYelFillAmt;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");

        playerController = player.GetComponent<playerController>();
        playerSpawnPOS = GameObject.FindWithTag("Player Spawn Pos");
        currtime = Time.timeScale;

        levelTime = timer.GetComponent<levelTimer>();
        if (levelTime != null)
            timer.SetActive(levelTime.timerNeeded);
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

    public void moveHPBar()
    {
        if (healthYel.fillAmount > healthRed.fillAmount)
        {
            if (healthYel.fillAmount - healthRed.fillAmount > .2)
                healthYel.fillAmount = Mathf.Lerp(healthYel.fillAmount, healthRed.fillAmount, Time.deltaTime);
            else
                healthYel.fillAmount -= (healthYelFillAmt - healthRedFillAmt) * Time.deltaTime * 2;
        }
    }

    public void moveStamBar()
    {
        if (stamYel.fillAmount > stamBlue.fillAmount && (stamYelFillAmt - stamBlueFillAmt) >.01)
        {
            if (stamYel.fillAmount - stamBlue.fillAmount > .08)
                stamYel.fillAmount = Mathf.Lerp(stamYel.fillAmount, stamBlue.fillAmount, Time.deltaTime*4);
            else
                stamYel.fillAmount -= (stamYelFillAmt - stamBlueFillAmt) * (Time.deltaTime * 4);
        }
        else
        {
            stamYel.fillAmount = stamBlue.fillAmount;
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
    public void updatGameGoal(int update, bool isEnemy = true)
    {
        if(isEnemy)
            enemiesalive += update;
        else
            pickupsLeft += update;
        objectiveText.text = updateObjective();

        if(enemiesalive <= 0 && pickupsLeft <= 0 && isInsideExit == true)
        {
            StartCoroutine(youWin());
        }
    }
    public IEnumerator youWin()
    {
        timer.SetActive(false);
        yield return new WaitForSeconds(1);
        statePause();
        activeMenu = winMenu;
        activeMenu.SetActive(isPause);
    }

    public string updateObjective()
    {
        string buffer;
        if (enemiesalive <= 0)
            buffer = "Objectives: " + "\nComplete!";
        else
            buffer = "Objectives: " + "\nEnemies Remaining: " + enemiesalive;
        if (pickupsLeft <= 0)
            buffer += "\nComplete!";
        else
            buffer += "\nPickups left: " + pickupsLeft;
        return buffer;
    }
    public void loseScreen()
    {
        if(activeMenu != loseMenu)
        {
            statePause();
            activeMenu = loseMenu;
            activeMenu.SetActive(isPause);
        }

    }

    public void isExiting(bool state)
    {
        isInsideExit = state;
    }

    public IEnumerator checkpointPopup()
    {
        checkpointMenu.SetActive(true);
        yield return new WaitForSeconds(2);
        checkpointMenu.SetActive(false);
    }
}
