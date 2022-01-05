using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTaskScript : MonoBehaviour
{
    public Collider player;
    public GameplayManager gameplayManager;
    public StarBoardSideTriggerScript starBoardSideTriggerScript;
    public bool didPlayerInteract = false;
    public int eventNum = 0;

    private void Start()
    {
        didPlayerInteract = false;
        starBoardSideTriggerScript.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        eventNum++;
        if(eventNum > 2)
        {
            starBoardSideTriggerScript.gameObject.SetActive(true);
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if(other == player)
        {
            
            // If pressing E
            if(Input.GetAxis("Interact") >= 0.1f)
            {
               didPlayerInteract = true;
            }
            
        }
    }
    private void OnDisable()
    {
        didPlayerInteract = false;
    }
}
