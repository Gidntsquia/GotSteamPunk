using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointVoiceLineManager : MonoBehaviour
{
    public List<sound> checkpoints;
    public audioPlayer audioPlayer; 
    private int cannonLineNum = 1;

    void Awake()
    {
        foreach(sound sound in checkpoints)
        {
            audioPlayer.setSoundValues(sound);
        }
    }

    // Checkpoints list:
    // 0: Intro
    // 1: Outro
    // 2: Cannon initiation 1
    // 3: Cannon initiation 2
    // 4: Cannon everything fails

    // This plays the intro voice line.
    public void playIntroLine()
    {
        audioPlayer.spawnCaptainInterrupt(checkpoints[0]);      
    }

    // This plays the outro voice line.
    public void playOutroLine()
    {
        audioPlayer.spawnCaptainInterrupt(checkpoints[1]);
    }

    // This plays the desired Cannon-related voice line.
    public void playCannonLine(int lineNum)
    {
        // Line num-
        //   1: Cannon initiation 1
        //   2: Cannon initiation 2
        //   3: Cannon everything fails
        if(lineNum >= 3)
        {
            print("Cannon line was too high. It was: " + lineNum);
            lineNum = 3;
        }
        audioPlayer.spawnCaptainInterrupt(checkpoints[1 + lineNum]);
    }
}
