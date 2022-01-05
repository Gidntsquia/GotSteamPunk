using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task4Script : MonoBehaviour
{
    // NOTE: Make sure the tasks are enabled in the scene editor, or else this script will not play correctly.
    // TODO figure out how to make something hookable or not. Make damaged pillar unhookable.
    // TODO add voicelines.
    public GameObject mainPillar;
    public GameObject damagedPillar;
    public bool shouldPillarBeHookable = true;

    void OnEnable()
    {
        enablePillarHookability();
    }

    void Update()
    {
        // This checks for if the task was failed and activates the unhookable pillar if so. 
        // Will not reactivate pillar's hookability until the task is completed.
        if(!GetComponent<IndividualTaskScript>().isCompletable)
        {
            disablePillarHookability();
        }
        
    }


    void OnDisable()
    {
        enablePillarHookability();
    }
    
    // This enables the main pillar asset and disables the damaged pillar asset since only one can be active at a time.
    private void enablePillarHookability()
    {
        //mainPillar.SetActive(true);
        //damagedPillar.SetActive(false);
        //shouldPillarBeHookable = true;
        
        foreach(Transform pillarPart in mainPillar.GetComponentsInChildren<Transform>())
        {
            pillarPart.gameObject.layer = 0;
        }
    }


    // This enables the damaged pillar asset and disables the main pillar asset since only one can be active at a time.
    private void disablePillarHookability()
    {
        //mainPillar.SetActive(false);
        //damagedPillar.SetActive(true);
        //shouldPillarBeHookable = false;
        // This makes the pillar part of the "unhookable" layer and therefore unhookable.
        foreach(Transform pillarPart in mainPillar.GetComponentsInChildren<Transform>())
        {
            pillarPart.gameObject.layer = 8;
        }
    } 
    
}
