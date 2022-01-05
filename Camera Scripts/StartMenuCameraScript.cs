using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuCameraScript : MonoBehaviour
{
    public Camera playerCamera;
    public GameManager gameManager;
    
    void Start()
    {
        this.GetComponent<Animator>().speed = 0f;
    }

    public Vector3 getPlayerCameraPos()
    {
        return playerCamera.gameObject.transform.position;
    }

    public void playStartGameAnimation()
    {
        //this.GetComponent<Animator>().Play("moveToPlayer", -1);
        this.GetComponent<Animator>().speed = 1f;
    }

    public void sendStartGameEvent()
    {
        gameManager.startGame();
    }


}
