using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float constTimer =15;
    public float time = 15;
    public bool isPaused = true;
    TaskGroupManager taskManager;
    TMPro.TextMeshProUGUI timerText;
    private roundFail failScript;
    public bool runningAnim = false;

    void Start()
    {
        failScript = GameObject.Find("roundTimer").GetComponent<roundFail>();
        timerText = GameObject.FindGameObjectWithTag("TimerText").GetComponent<TMPro.TextMeshProUGUI>();
        taskManager = GameObject.FindGameObjectWithTag("TaskGroupManager").GetComponent<TaskGroupManager>();
        time = constTimer;
    }

    void Update()
    {
        
        if(!isPaused)
        {
            if(time > 0 && !runningAnim){
            time -= Time.deltaTime;
            timerText.SetText(time.ToString().Substring(0,4));
        }
        else
        {
            timerText.SetText("0.00");
            if(taskManager.taskAdvancement > 0)
            {
                isPaused = true;
            }
            if(!runningAnim)
            {
            failScript.startFail();
            }
            time = constTimer;
        }
        }
        
    }
    public void UnPause()
    {
        isPaused = false;
    }
}
