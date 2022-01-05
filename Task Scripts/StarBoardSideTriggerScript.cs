using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBoardSideTriggerScript : MonoBehaviour
{
    public audioPlayer audioPlayer;
    public Collider player;
    private bool hasTriggered = false;

    private void OnEnable()
    {
        hasTriggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other == player && !hasTriggered)
        {
            hasTriggered = true;
            audioPlayer.spawnCaptainInterrupt("starBoard");
        }
    }
}
