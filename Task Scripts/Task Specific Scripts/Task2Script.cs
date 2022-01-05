using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task2Script : MonoBehaviour
{
    // TODO add voice lines
    // Has two interaction points.

    public IndividualTaskScript secondInteractionPoint;
    private IndividualTaskScript mainInteractionPoint;
    private string currentInteractionPoint;

    void Awake()
    {
        mainInteractionPoint = GetComponent<IndividualTaskScript>();
        secondInteractionPoint.secondsToComplete = mainInteractionPoint.secondsToComplete;
        secondInteractionPoint.failLockSeconds = mainInteractionPoint.failLockSeconds;
    
    }

    void OnEnable()
    {
        enableSecondInteraction();
    }

    // Update is called once per frame
    void Update()
    {   
        // This will only enable main interaction point once.
        if(secondInteractionPoint.isTaskCompleted && secondInteractionPoint.isCompletable)
        {
            enableMainInteraction();
        }

        // Checks if the main interaction point failed. Will only work if current interaction point is the main one.
        // This will only run once since "failThisTask" sets the current interaction point to second. This quick fix 
        // doesn't work on the Task 3 Script, so this is handled differently differently there. 
        if(currentInteractionPoint.Equals("main") && !mainInteractionPoint.isCompletable)
        {
            failThisTask();
        }
    }

    // This makes main task interaction point able to be completed.
    // Disables second interaction point. 
    void enableMainInteraction()
    {
        mainInteractionPoint.isCompletable = true;
        secondInteractionPoint.isCompletable = false;

        // These activate/deactivate the minimap icon.
        mainInteractionPoint.transform.GetChild(0).gameObject.SetActive(true);
        secondInteractionPoint.transform.GetChild(0).gameObject.SetActive(false);

        // Continues timer between interaction points.
        mainInteractionPoint.secondsEnabled = secondInteractionPoint.secondsEnabled;

        currentInteractionPoint = "main";
    }

    // This makes second task interaction point able to be completed. 
    // Disables main interaction point.
    void enableSecondInteraction()
    {
        mainInteractionPoint.isCompletable = false;
        secondInteractionPoint.isCompletable = true;

        // These activate/deactivate the minimap icon.
        mainInteractionPoint.transform.GetChild(0).gameObject.SetActive(false);
        secondInteractionPoint.transform.GetChild(0).gameObject.SetActive(true);

        currentInteractionPoint = "second";

    }

    // This will reset this task by hard resetting the main interaction point and allowing the second interaction point to do the fail lockout.
    private void failThisTask()
    {
        mainInteractionPoint.StopAllCoroutines();
        secondInteractionPoint.StopAllCoroutines();

        mainInteractionPoint.GetComponent<Collider>().enabled = true;
        
        // This deactivates the minimap icon.
        mainInteractionPoint.transform.GetChild(0).gameObject.SetActive(false);
        currentInteractionPoint = "second";

        StartCoroutine(secondInteractionPoint.failTask());

        mainInteractionPoint.secondsEnabled = 0f;
        secondInteractionPoint.secondsEnabled = 0f;
        mainInteractionPoint.isCompletable = false;
        secondInteractionPoint.isCompletable = false;
        secondInteractionPoint.isTaskCompleted = false;

        
    }
}
