using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task6Script : MonoBehaviour
{
    private IndividualTaskScript individualTaskScript;
    
    private void Start()
    {
        individualTaskScript = GetComponent<IndividualTaskScript>();    
    }
    void Update()
    {
        // Fail the round if this task is failed.
        if(!individualTaskScript.isCompletable)
        {
            print("Task 6 was failed! Restarting the round.");
            GetComponentInParent<GameplayManager>().sendRoundFailEvent();
        }
    
    }
}
