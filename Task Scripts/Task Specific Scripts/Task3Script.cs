using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task3Script : MonoBehaviour
{
    // TODO add voice lines
    // TODO make it impossible to hook on water spill.
    // This task has two interaction points. Also, water will spill if this task is failed.
    public GameObject waterSpillAsset;
    private IndividualTaskScript mainInteractionPoint;

    void Awake()
    {
        mainInteractionPoint = GetComponent<IndividualTaskScript>();
        disableWaterSpill();
    }


    void OnEnable()
    {
        disableWaterSpill();
    }
        

    void OnDisable()
    {
        disableWaterSpill();
    }

    private void enableWaterSpill()
    {
        waterSpillAsset.SetActive(true);
    }

    private void disableWaterSpill()
    {
        waterSpillAsset.SetActive(false);
    }

    // This will reset this task by hard resetting the main interaction point and allowing the second interaction point to do the fail lockout.
    // This will also enable the water spill, which will only disappear upon completing the task. 
    public void failThisTask()
    {
        print("failed");
        mainInteractionPoint.StopAllCoroutines();

        mainInteractionPoint.GetComponent<Collider>().enabled = true;
        
        // This deactivates the minimap icon.
        mainInteractionPoint.transform.GetChild(0).gameObject.SetActive(false);
        mainInteractionPoint.secondsEnabled = 0f;
        mainInteractionPoint.isCompletable = false;
        enableWaterSpill();
        
    }

}
