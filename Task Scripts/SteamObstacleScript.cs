using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamObstacleScript : MonoBehaviour
{
    public static bool doingInitialSpeedBurst = false;
    public ParticleSystem steamParticles;
    public SteamBarScript steamBarScript;
    public Collider player;
    public float steamSpewingSeconds = 3f;
    public float steamOffSeconds = 3f;
    public float steamBarSpeedIncreasePercent = 0.5f;
    public bool isSpewing = false;

    private void OnEnable()
    {
        StartCoroutine(oscillateBetweenSpewingAndOff());
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other == player)
        {
            
            
            
            // Increases speed in general, and also gives a burst of speed. 
            // This makes it where the speed bar speeds up even if you're only in the steam
            // for a little bit.
            steamBarScript.stopSpeedBurst();
            steamBarScript.startSpeedBurst(4f, 1f);
            steamBarScript.increaseSpeedBy(steamBarSpeedIncreasePercent);
           
            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other == player)
        {
            // speed up steam bar
            


        }    
    }

    private void OnTriggerExit(Collider other)
    {
        if(other == player)
        {
            
            steamBarScript.decreaseSpeedBy(steamBarSpeedIncreasePercent);
        }
    }


    private void OnDisable()
    {
        steamBarScript.resetSteambarSpeed();
        isSpewing = false;
        doingInitialSpeedBurst = false;
        this.GetComponent<Collider>().enabled = false;
        StopAllCoroutines();
        stopSpewing();
    }

    private IEnumerator oscillateBetweenSpewingAndOff()
    {
        // This oscellates between the steam obstacle spewing and being off.
        while(true)
        {
            startSpewing();
            yield return new WaitForSeconds(steamSpewingSeconds);

            stopSpewing();
            yield return new WaitForSeconds(steamOffSeconds);
        }
    }

    private void startSpewing()
    {
        
        steamParticles.Play();
        this.GetComponent<Collider>().enabled = true;
        this.GetComponent<Animator>().SetBool("shouldSpew", true);
        isSpewing = true;
    }

    private void stopSpewing()
    {
        steamParticles.Stop();
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<Animator>().SetBool("shouldSpew", false);
        isSpewing = true;
    }
}
