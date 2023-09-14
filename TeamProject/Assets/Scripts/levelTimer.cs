using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class levelTimer : MonoBehaviour
{

    [SerializeField] float timeLimit;
    public float timeBuff;
    public float timeTaken;
    public bool timerNeeded;
    [SerializeField] TMP_Text timeText;
    
    
    // Start is called before the first frame update
    void Start()
    {
        timeTaken = timeLimit;
        timeBuff = timeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        timeTaken -= Time.deltaTime;
        timeText.text = formatTime();
        if (timeTaken <= 0)
            GameManager.instance.loseScreen();
    }

    public string formatTime()
    {
        int minutes = (int)timeTaken / 60;
        int seconds = (int)timeTaken % 60;

        string totTime = "[" + minutes.ToString() + ":" + seconds.ToString() + "]";
        return totTime;
    }
}
