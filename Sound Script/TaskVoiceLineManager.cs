using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskVoiceLineManager : MonoBehaviour
{
    public List<VoiceLineTypeWrapper> taskVoicelines;
    public audioPlayer audioPlayer;
    public bool hasTask3FailedOnce = false;
    public float probabilityOfPlayingFollowUp = 0.333f;
    public List<bool> isTaskFirstActivationList = new List<bool>(6);
    private int currentTask = 0;
    

    // Each task represents itself, minus one. Task 1 is element 0 and so on.
    // Each task has 3 voice line types. 
    // Voice line types:
    //      0: Initiation
    //      1: Follow up
    //      2: Fail
    // Each "type" has a list of sounds associated with it. Sometimes, the fail list is empty.

    void Awake()
    {
    
        hasTask3FailedOnce = false;
        foreach(VoiceLineTypeWrapper voiceLineType in taskVoicelines)
        {
            foreach(SoundWrapper sounds in voiceLineType.voiceLineType)
            {
                foreach(sound sound in sounds.sounds)
                {
                    audioPlayer.setSoundValues(sound);
                }
            }
        }

        // This sets up each task as having being on its first activation.
        // There must be 6 indexes in the isTaskFirstActivationList list in the editor for this to work.
        for(int i = 0; i < 6; i++)
        {
            isTaskFirstActivationList[i] = true;
        }
        

        
    }


    public void playTaskSpawnSound(int task)
    {
        // A 1 second wait is done to give breathing room after a task is finished.
        StartCoroutine(doWaitBeforePlayingSound(task, 1f));
        
    }

    public void playFailSound(int task)
    {
        // Special behavior for task 3. TODO get rid of this.
        if(task == 2)
        {
            if(!hasTask3FailedOnce)
            {
                audioPlayer.spawnCaptainInterrupt(taskVoicelines[task].voiceLineType[2].sounds[0]);
                hasTask3FailedOnce = true;
            }
            else
            {
                audioPlayer.spawnCaptainInterrupt(taskVoicelines[task].voiceLineType[2].sounds[1]);     
            }
        }
        else if(taskVoicelines[task].voiceLineType[2].sounds.Count != 0)
        {
            int randomIndex = Random.Range(0, taskVoicelines[task].voiceLineType[2].sounds.Count);
            audioPlayer.spawnCaptainInterrupt(taskVoicelines[task].voiceLineType[2].sounds[randomIndex]);
        }
    }

    private IEnumerator doWaitBeforePlayingSound(int task, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if(isTaskFirstActivationList[task])
        {
            audioPlayer.spawnCaptainInterrupt(taskVoicelines[task].voiceLineType[0].sounds[0]);
            isTaskFirstActivationList[task] = false;
        }
        else
        {
            // Will only play follwup some of the time.
            float randomValue = Random.value;
            if(randomValue <= probabilityOfPlayingFollowUp)
            {
                // This will play a random sound in the follow up voice lines list.
                int randomIndex = Random.Range(0, taskVoicelines[task].voiceLineType[1].sounds.Count);
                audioPlayer.spawnCaptainInterrupt(taskVoicelines[task].voiceLineType[1].sounds[randomIndex]);
            }
            
        }
    }

    // Returns the length of the initialization voice line of the water spill task.
    public float getTask3InitializationVoiceLineLength()
    {
        return taskVoicelines[2].voiceLineType[0].sounds[0].clip.length;
    }

    // Returns the length of the first fail voice line of the water spill task.
    public float getTask3FirstFailureVoiceLineLength()
    {
        return taskVoicelines[2].voiceLineType[2].sounds[0].clip.length;
    }

    


}
