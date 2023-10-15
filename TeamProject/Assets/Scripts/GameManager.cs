using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager instance;

    [Header("----- Player Data -----")]
    public GameObject player;
    public playerController playerController;
    public GameObject playerSpawnPOS;
    public GameObject playerGun;
    public Gun playerGunScript;
    public GameObject playerGrenadePickup;
    public PlayerGrenade playerGrenadeGM;
    public DisplayInventory displayInventory;
    public Camera mainCamera;

    [Header("----- Menus -----")]
    [SerializeField] GameObject activeMenu;
    [SerializeField] GameObject lastMenu;
    [SerializeField] AudioSource mainMusic;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] GameObject checkpointMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject creditsMenu;
    [SerializeField] GameObject saveOverlay;

    [Header("----- Play State -----")]
    [SerializeField] GameObject endPoint;
    [SerializeField] GameObject playUI;
    [SerializeField] GameObject timer;
    [SerializeField] TMP_Text objectiveText;
    [SerializeField] TMP_Text ammoCount;
    public levelTimer levelTime;
    public Image healthRed;
    public Image healthYel;
    public Image stamBlue;
    public Image stamYel;

    public int enemiesalive;
    int pickupsLeft;

    //I expect the player person to set this up I'm just putting it here for later

    public bool isPause;
    float currtime;

    bool isInsideExit;

    public float healthRedFillAmt;
    public float healthYelFillAmt;
    public float stamBlueFillAmt;
    public float stamYelFillAmt;
    public int levelClearedAmount;
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

        playerGun = GameObject.FindGameObjectWithTag("Player Gun");
        playerGunScript = playerGun.GetComponent<Gun>();

        playerGrenadePickup = GameObject.FindGameObjectWithTag("Grenade PickUp");
        playerGrenadeGM = playerGrenadePickup.GetComponent<PlayerGrenade>();

        displayInventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<DisplayInventory>();

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Start()
    {
        optionsMenu.SetActive(false);
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
        mainMusic.volume = .4f;
        playUI.SetActive(!isPause);
    }

    public void setMenu(GameObject menu)
    {
        activeMenu = menu;
    }

    public void SaveOverlay(bool on)
    {
        saveOverlay.SetActive(on);
    }

    public void options()
    {
        lastMenu = activeMenu;
        setMenu(optionsMenu);
        lastMenu.SetActive(false);
        activeMenu.SetActive(true);
    }

    public void credits()
    {
        lastMenu = activeMenu;
        setMenu(creditsMenu);
        lastMenu.SetActive(false);
        activeMenu.SetActive(true);
    }
    public void back()
    {
        activeMenu.SetActive(false);
        setMenu(lastMenu);
        lastMenu = null;
        activeMenu.SetActive(true);
    }

    public void stateUnpause()
    {
        Time.timeScale = currtime;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPause = !isPause;
        mainMusic.volume = 1;
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
        setMenu(winMenu);
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
    public void loseScreen(string death)
    {
        if(activeMenu != loseMenu)
        {
            statePause();
            setMenu(loseMenu);
            if(death != null)
            {
                TextMeshProUGUI text = loseMenu.GetComponentInChildren<TextMeshProUGUI>();
                text.text = "You were " + death + "!";
            }
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

    public void ammoUpdate(int amount, int maxAmount, bool reload = false)
    {
        if (maxAmount >= 1000)
        {
            ammoCount.text = "INFINITE";
            return;
        }

        if (reload)
        {
            ammoCount.text = "Reloading!";
            return;
        }
        ammoCount.text = amount.ToString() + " / " + maxAmount.ToString();
    }
    public void LoadData(GameData data)
    {
        instance.playerSpawnPOS.transform.position = data.playerPos;
        instance.levelClearedAmount = data.levelCount;
    }

    public void SaveData(GameData data)
    {
        data.playerPos = instance.playerSpawnPOS.transform.position;
    }

    public void PlaySound(AudioSource source, float low = .7f, float high = 1.3f)
    {
        AudioChange(source, low, high);
        source.Play();
        StartCoroutine(clipEnd(source, source.clip.length));
    }

    public void AudioChange(AudioSource audio, float low = .7f, float high = 1.3f)
    {
        float rand = UnityEngine.Random.Range(0, 100);
        if (rand > 66)
            audio.pitch *= high;
        else if (rand > 33)
            audio.pitch *= low;
        else
            return;
    }

    public IEnumerator clipEnd(AudioSource source, float length, float origPitch = 1)
    {
        yield return new WaitForSeconds(length);
        source.pitch = origPitch;
    }
}
