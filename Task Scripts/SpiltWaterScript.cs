using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiltWaterScript : MonoBehaviour
{
    public Collider player;
    public tipCollision tipCollision;
    private float startingCounterMovement;

    private void Awake()
    {
        startingCounterMovement = player.gameObject.GetComponent<PlayerMovement>().counterMovement;
    }

    private void OnEnable()
    {
        tipCollision.canHook = true;
        player.gameObject.GetComponent<PlayerMovement>().counterMovement = startingCounterMovement;
    }
    private void OnTriggerEnter(Collider other)
    {

        if(other == player)
        {
            
            tipCollision.canHook = false;
            player.gameObject.GetComponent<PlayerMovement>().counterMovement = 0.003f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other == player)
        {
            tipCollision.canHook = true;
            player.gameObject.GetComponent<PlayerMovement>().counterMovement = startingCounterMovement;
        }
    }

    private void OnDisable()
    {
        tipCollision.canHook = true;
        player.gameObject.GetComponent<PlayerMovement>().counterMovement = startingCounterMovement;
    }
}
