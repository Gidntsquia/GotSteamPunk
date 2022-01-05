using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskGroupManager : MonoBehaviour
{
    //add timer fucntionality
    //use the time delta
    private List<GameObject> tasks = new List<GameObject>();
    public List<int> currentTasks = new List<int>();
    // There must always be the same number of elements in clumpAutospawn Seconds and taskOrder to correspond correctly.
    public List<RoundWrapper> taskOrder = new List<RoundWrapper>();  
    public List<RoundTimerWrapper> clumpAutospawnSeconds = new List<RoundTimerWrapper>();
    public int roundAdvancement = 0;
    public int clumpAdvancement = 0;
    public int taskAdvancement = 0;
    private int waitValue = -1;

    void Awake()
    {
        
        // This code will not work in "Start()" for reasons I do not know.
        tasks = new List<GameObject>(GameObject.FindGameObjectsWithTag("Task"));
        tasks.Sort(CompareTasks);
        foreach(GameObject task in tasks)
        {
            task.SetActive(false);
        }
        clumpAdvancement = 0;
        roundAdvancement = 0;
        
    }


    void OnEnable()
    {
        if(GetComponentInParent<GameplayManager>().autoStartNextTasks)
        {
            //spawnClump(clumpAdvancement);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if any tasks are completed.
        if(currentTasks.Count > 0)
        {
            if(currentTasks[0] != waitValue)
            {
                foreach(int task in currentTasks)
                {
                    if(tasks[task].GetComponent<IndividualTaskScript>().isTaskCompleted == true)
                    {
                        resetTask(task);
                        break;  
                    }
                }
            }
            
        }
        
    }

    public void goNextTaskClump()
    {
        // Activate when tasks at 0 or when timer in GameplayManager sends a call
        // Spawn next clump in the round. Will only update/spawn clump if next clump is available. 
        if(clumpAdvancement + 1 < taskOrder[roundAdvancement].round.Count)
        {
            clumpAdvancement++;
            spawnClump(clumpAdvancement);
        }

    
    }

    public void goNextRound()
    {
        // Will go to the next round and spawn the first clump.
        resetRound(roundAdvancement);
        
        // Will only advance the round if next round is available.
        if(roundAdvancement + 1 < taskOrder.Count)
        {
            roundAdvancement++;
            spawnClump(0);
        }
        
        
    }

    public void spawnClump(int clump)
    {
        // Will only spawn clump if its available 
        if(clump < taskOrder[roundAdvancement].round.Count)
        {
            bool playingSound = false;
            taskOrder[roundAdvancement].round[clumpAdvancement].clump.Shuffle();

            // Finds the desired clump in the current round in the list of rounds.
            foreach(int task in taskOrder[roundAdvancement].round[clumpAdvancement].clump)
            {
                spawnTask(task);
                if(GetComponent<TaskVoiceLineManager>().isTaskFirstActivationList[task] == true && !playingSound && roundAdvancement != 8)
                {
                    GetComponent<TaskVoiceLineManager>().playTaskSpawnSound(task);
                    playingSound = true;
                }
            }

            // This will give an equal chance to play the followup of each task in the round.
            if(!playingSound && roundAdvancement != 8)
            {
                GetComponent<TaskVoiceLineManager>().playTaskSpawnSound(taskOrder[roundAdvancement].round[clumpAdvancement].clump[0]);
            }
        }
        
    }

    public void spawnTask(int task)
    {
        // Only adds task if its not already there.
        if(!currentTasks.Contains(task))
        {
            currentTasks.Add(task);
        }
        tasks[task].SetActive(true);
    }

    public void resetTask(int task)
    {

        // Resets one task by deactivating it and setting it up to be used again. 
        if(currentTasks.Contains(task))
        {
            currentTasks.Remove(task);
        }
        GameObject taskObject = tasks[task];
        taskObject.GetComponent<IndividualTaskScript>().isTaskCompleted = false;

        // Task 2 and 3 have two interaction points, so they spawn in as not completable.
        if(task == 1 || task == 2)
        {
            taskObject.GetComponent<IndividualTaskScript>().isCompletable = false;
        }
        else
        {
            taskObject.GetComponent<IndividualTaskScript>().isCompletable = true;
        }
        taskObject.GetComponent<Collider>().enabled = true;
        taskObject.SetActive(false);
        
    }

    public void resetClump(int theClump)
    {
        // Resets all tasks in the clump in the current round.
        foreach(int task in taskOrder[roundAdvancement].round[theClump].clump)
        {
            resetTask(task);
        }
    }

    public void resetRound(int theRound)
    {
        // Resets all tasks in current clump in the round.
        // This is in case multiple clumps are active at once
        foreach(ClumpWrapper clump in taskOrder[roundAdvancement].round)
        {
            foreach(int task in clump.clump)
            {
                resetTask(task);
            }
        }
        currentTasks.Clear();
        StopAllCoroutines();
        clumpAdvancement = 0;
    }

    public void failRound()
    {
        // Resets the round
        resetRound(roundAdvancement);
        if(roundAdvancement <= 1)
        {
            roundAdvancement = 0;
        }
        else if(roundAdvancement <= 4)
        {
            roundAdvancement = 2;
        }
        else if(roundAdvancement <= 8)
        {
            roundAdvancement = 5;
        }
        
    }

    public bool isOnLastClump()
    {
        // Returns if the current clump is the last one.
        return clumpAdvancement + 1 >= taskOrder[roundAdvancement].round.Count;
    }

    public bool isOnLastRound()
    {
        return roundAdvancement + 1 >= taskOrder.Count;
    
    }

    public int getClumpAutoSpawnSeconds()
    {
        //print(roundAdvancement);
        //print(clumpAdvancement);
        return clumpAutospawnSeconds[roundAdvancement].clumpTimersInRound[clumpAdvancement];
    }

    public void printCurrentTasks()
    {
        foreach(int task in currentTasks)
        {
            print(task);
        }
    }

    public int CompareTasks(GameObject first, GameObject other)
    {
        return first.GetComponent<IndividualTaskScript>().taskNumber.CompareTo(other.GetComponent<IndividualTaskScript>().GetComponent<IndividualTaskScript>().taskNumber);
        
    }
}



// This code is from Smooth.foundations from Smooth-P.
// https://forum.unity.com/threads/clever-way-to-shuffle-a-list-t-in-one-line-of-c-code.241052/
public static class IListExtensions {
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts) {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}

    

    

