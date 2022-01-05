using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class SecondInteractionScript : MonoBehaviour
{

    public float completionSeconds = 1;
    public float completionDistance = 4;
    public bool isTaskCompleted = false;
    public bool isCompletable = true;
    public float secondsEnabled = 0f;
    public IndividualTaskScript parentTask;
    private Transform completionObject;
    private int taskNumber;
    private Transform UIHolder;
    private Collider player;
    private float secondsToComplete = 30;
    private float failLockSeconds = 5;
    private bool firstPress = true;
    private float startTime = 0f;
    private Image progressBar;
    private Transform canvasClone;
    
    

    void Start()
    {
        
        completionObject = this.GetComponent<Transform>();
        taskNumber = parentTask.taskNumber;
        UIHolder = parentTask.UIHolder;
        player = parentTask.player;
        //completionObject.localScale = Vector3.one * completionDistance;
        progressBar = UIHolder.GetComponentInChildren<Image>();
        progressBar.fillAmount = 0f;
        firstPress = true;
        
    }
    
    void OnEnable()
    {
        // Resets timer every time the task becomes active.
        secondsEnabled = 0f;
        isTaskCompleted = false;
        isCompletable = true;
        parentTask.isCompletable = false;
        parentTask.transform.GetChild(0).gameObject.SetActive(false);
        parentTask.transform.GetChild(1).gameObject.SetActive(false);
        this.transform.GetChild(0).gameObject.SetActive(true);
        this.transform.GetChild(1).gameObject.SetActive(true);
    }

    void FixedUpdate()
    {
        if(isCompletable || isTaskCompleted)
        {
            // This tracks how long the task has been active. After secondsToComplete seconds have passed, fail the task.
            secondsEnabled += Time.deltaTime; 
            if(secondsEnabled >= parentTask.secondsToComplete)
            {
                StopAllCoroutines();
                StartCoroutine(failTask());
                secondsEnabled = 0f;
            }
        }
        
        
        
    }

    // This will run while the player is in the task hitbox.
    private void OnTriggerStay(Collider other)
    {
        if(other == player)
        {
            //if close to task and task isnt completed. Also, if task is completeable.
            if(!isTaskCompleted && isCompletable)
            {
                // If pressing E
                if(Input.GetAxis("Interact") >= 0.1f)
                {
                    // If pressing e for the first time near task
                    if(firstPress)
                    {
                        // Calling the coroutine as a String makes it restart when we do "StopCoroutine."
                        // StopCoroutine behavior:
                        //      String    --> restarts
                        //      Variable  --> Starts where it left off
                        //      Method    --> Doesn't work
                        StartCoroutine("doTask");
                        startTime = Time.time;
                        firstPress = false;
                    }

                    // Gets progress towards completing task as value from 0 - 1.
                    float progress = (Time.time - startTime) / parentTask.completionSeconds;
                
                    progressBar.fillAmount = progress;
                }
                else
                {
                    stopTask();
                }
            }
        }
        
    }

    // This will run once when the player leaves the task hitbox.
    private void OnTriggerExit(Collider other)
    {
        if(other == player)
        {
            stopTask();
        }
    }

    

    // Checks if player is still holding "Interact" after "completionSeconds" time. Independent timer from bar. 
    public IEnumerator doTask()
    {
        // TODO Play sound here
        //create a new image holder at said position with no rotation with said task as the parent
        canvasClone = Instantiate(UIHolder, completionObject.position + Vector3.up * 2 + Vector3.forward * 5, Quaternion.Euler(0, 0, 0), completionObject);
        progressBar = canvasClone.GetComponentInChildren<Image>(); 
        yield return new WaitForSeconds(parentTask.completionSeconds);
        if(Input.GetAxis("Interact") >= 0.1f && !isTaskCompleted)
        {

            isCompletable = false;
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(false);

            parentTask.isCompletable = true;
            parentTask.secondsEnabled = -100f;
            isTaskCompleted = true;
            parentTask.transform.GetChild(0).gameObject.SetActive(true);
            parentTask.transform.GetChild(1).gameObject.SetActive(true);
            
            stopTask();
            print(completionObject.name + " second interaction completed!");
            
        }
    }

    public IEnumerator failTask()
    {
        // Lock task for a few seconds (changes)
        // Increase steam bar a bit

        // Disables the task's collider temporarily to ensure it can't be completed.
        print("Failed task!");
        GetComponentInParent<GameplayManager>().sendTaskFailEvent(taskNumber);
        if(taskNumber == 2)
        {
            parentTask.gameObject.GetComponent<Task3Script>().failThisTask();
        }
        stopTask();
        this.GetComponent<Collider>().enabled = false;
        isTaskCompleted = false;
        parentTask.isCompletable = false;
        isCompletable = false;
        // Deactivates the minimap indicator. The minimap indicator must be a child of the task.
        this.transform.GetChild(0).gameObject.SetActive(false);
        this.transform.GetChild(1).gameObject.SetActive(false);
        parentTask.transform.GetChild(0).gameObject.SetActive(false);
        parentTask.transform.GetChild(1).gameObject.SetActive(false);
        if(this.transform.GetChild(0).gameObject.layer != 7)
        {
            print("This script probably thinks the wrong object is the minimap indicator, which means that it likely deactivated the wrong thing. Just saying.");
        }

        yield return new WaitForSeconds(parentTask.failLockSeconds);
        secondsEnabled = 0f;
        isCompletable = true;
        
        this.GetComponent<Collider>().enabled = true;
        // Reactivates the minimap indicator.
        this.transform.GetChild(0).gameObject.SetActive(true);
        this.transform.GetChild(1).gameObject.SetActive(true);
        print("Back online: " + name);
    }

    public void stopTask()
    {
        // Resets ability to "do" task.
        firstPress = true;
        StopCoroutine("doTask");
        if(progressBar != null)
        {
            progressBar.fillAmount = 0f;    
        }
        if(canvasClone != null)
        {
            Destroy(canvasClone.gameObject);
        }
        
    }

    private void OnDisable()
    {
        stopTask();
    }

}
