using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteamBarScript : MonoBehaviour
{
    // Value from 0 to 1 representing how full the steam bar is.
    public float fillProgress = 0f;
    public float secondsToFillMax = 100f;
    public float fillSpeed = 1.0f;
    public Image steamBar;
    public bool isBarEnabled = true; 
    public bool isDoingSpeedBurst = false;
    private Coroutine speedBurstRoutine; 
    private float secondsEnabled;
    private float baseFillSpeed = 1f;
    private float x = 0f;
    

    // Start is called before the first frame update
    void Start()
    {
        steamBar.fillMethod = Image.FillMethod.Horizontal;
        steamBar.fillAmount = fillProgress;
        baseFillSpeed = 1f;
        
    }

    void OnEnable()
    {
        fillProgress = 0f;
        secondsEnabled = 0f;
        steamBar.fillMethod = Image.FillMethod.Horizontal;
        steamBar.fillAmount = fillProgress;
    }

    // Update is called once per frame
    void Update()
    {
        if(isBarEnabled)
        {
            // Seconds can't go below 0 and will only count up to max.
            secondsEnabled = Mathf.Clamp(secondsEnabled, 0f, secondsToFillMax);
            secondsEnabled += Time.deltaTime * fillSpeed;
            
            
            
            // Fill progress is the percent filled the bar is from 0 - 1.
            // Increases radically, but gets to (1, 1) at the same time as if the sqrt wasn't there.
            fillProgress = Mathf.Sqrt(secondsEnabled / secondsToFillMax);

            steamBar.fillAmount = fillProgress;
        }
        
        if(fillProgress >= 1.0f)
        {
            // Tells the Gameplay Manager that the steam bar is filled up.
            GetComponentInParent<GameplayManager>().sendRoundFailEvent();
            fillProgress = 0f;
        }
        

    }

    public void resetSteamBar()
    {
        // TODO Could have animation when it decreases to 0.
        fillProgress = 0f;
        secondsEnabled = 0f;
    }

    public void resetSteambarSpeed()
    {
        fillSpeed = baseFillSpeed;
    }

    // Progress is the percent filled the steam bar is (0-1).
    public void setSteamBarProgress(float progress)
    {
        fillProgress = Mathf.Clamp(progress, 0, 1);
        secondsEnabled = progress * secondsToFillMax;
    }

    // This method is called when a task is completed. Each task reduces the steam bar to various degrees.
    // Reduce by a float percentage, 0-1.
    public void reduceSteamBarProgress(float percent)
    {
        secondsEnabled =  secondsEnabled - secondsToFillMax * percent;
    }

    public void disableSteamBar()
    {
        // Saves "spot" of steam bar.
        this.transform.GetChild(0).gameObject.SetActive(false);
        isBarEnabled = false;
    }

    public void enableSteamBar()
    {
        // Enables the steam bar at the same "spot" that it was disabled at.
        this.transform.GetChild(0).gameObject.SetActive(true);
        isBarEnabled = true;
    }

    // Percent can't be -1.
    public IEnumerator speedUpSteamBar(float percent, float secondsForSpeed)
    {
        fillSpeed = baseFillSpeed * (1 + percent);
        yield return new WaitForSeconds(secondsForSpeed);
        fillSpeed = baseFillSpeed;
        
    }

    // This happens when the player gets hit by the steam obstacle.
    public IEnumerator doSpeedBurst(float percent, float secondsForSpeed)
    {

        if(!isDoingSpeedBurst)
        {
            fillSpeed = baseFillSpeed * (1 + percent);
            isDoingSpeedBurst = true;
        }
        
        yield return new WaitForSeconds(secondsForSpeed);

    
        fillSpeed = baseFillSpeed;
        isDoingSpeedBurst = false;
            
            
        
    }

    public void startSpeedBurst(float percent, float secondsForSpeed)
    {
        speedBurstRoutine = StartCoroutine(doSpeedBurst(percent, secondsForSpeed));
    }

    public void stopSpeedBurst()
    {
        if(speedBurstRoutine != null)
        {
            StopCoroutine(speedBurstRoutine);
        }
        
    }




    // Increases the steam bar's fill speed by the given percent. 
    public void increaseSpeedBy(float percent)
    {
        baseFillSpeed = baseFillSpeed * (1 + percent);
    }

    // Percent can't be -1.
    public void decreaseSpeedBy(float percent)
    {
        baseFillSpeed = baseFillSpeed  / (1 + percent);
    }

    public IEnumerator LerpSteamBarTo(float endProgress, float secondsToLerp)
    {
        float initialFillProgress = fillProgress;
        float endFillProgress = endProgress;
        
        print(fillProgress);
        // For the first 10th, it speeds up to the desired speed.
        float secondsElapsed = 0f;
        while(secondsElapsed <= secondsToLerp / 10)
        {
            fillProgress = Mathf.Lerp(initialFillProgress, endFillProgress, secondsElapsed / (secondsToLerp / 10f));
            secondsElapsed += Time.deltaTime;
            yield return null;
        }
        
        // Keeps desired speed for next 80% of time.
        yield return new WaitForSeconds(secondsToLerp * 0.8f);

        // For the last 10th, it slows down to the initial speed.
        secondsElapsed = 0f;
        while(secondsElapsed <= secondsToLerp / 10)
        {
            fillProgress = Mathf.Lerp(endFillProgress, initialFillProgress, secondsElapsed / (secondsToLerp / 10f));
            secondsElapsed += Time.deltaTime;
            yield return null;
        }

        // This ensures initial value was fully reached.
        fillProgress = initialFillProgress;
        print(fillProgress);
    }
}
