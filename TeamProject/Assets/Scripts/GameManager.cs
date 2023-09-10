using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

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
    public Image healthRed;
    public Image healthYel;

    int enemiesalive;

    //I expect the player person to set this up I'm just putting it here for later
    public GameObject playerSpawnPOS;

    bool isPause;
    float currtime;

    public float healthRedFillAmt;
    public float healthYelFillAmt;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        GameObject heRed = GameObject.FindGameObjectWithTag("RedHealth");
        if (heRed != null)
        {
            healthRed = heRed.GetComponent<Image>();
        }

        GameObject heYel = GameObject.FindGameObjectWithTag("YellowHealth");
        if (heYel != null)
        {
            healthYel = heYel.GetComponent<Image>();
        }

        playerController = player.GetComponent<playerController>();
        playerSpawnPOS = GameObject.FindWithTag("Player Spawn Pos");
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
    public void updatGameGoal(int enemies)
    {
        enemiesalive += enemies;
        if(enemiesalive <= 0)
        {
            StartCoroutine(youWin());
        }
    }
    IEnumerator youWin()
    {
        yield return new WaitForSeconds(1);
        statePause();
        activeMenu = winMenu;
        activeMenu.SetActive(isPause);
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
}
