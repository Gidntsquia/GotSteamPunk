using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameplayManager gameplayManager;
    public GameObject player;
    public GameObject startMenu;
    public Camera startMenuCamera;
    public bool isDebugMode = false;

    // Start is called before the first frame update
    void Start()
    {
        deactivateAll();
        startMenu.SetActive(true);
        startMenuCamera.gameObject.SetActive(true);
        gameplayManager.isDebugMode = isDebugMode;
        if(isDebugMode)
        {
            startGame();
            
        }
        gameplayManager.GetComponentInChildren<audioPlayer>().PlaySound("actualTitleMusic");
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        
    }

    


    private void deactivateAll()
    {
        gameplayManager.deactivateAll();
        player.gameObject.SetActive(false);
    }

    public void deactivateStartMenu()
    {
        startMenu.SetActive(false);
    }

    public void startGame()
    {
        print("starting game!");
        gameplayManager.GetComponentInChildren<audioPlayer>().stopSound("actualTitleMusic");
        if(GameObject.Find("ocean") != null)
        {
            GameObject.Find("ocean").transform.localScale = new Vector3(1, 1, 1);
            GameObject.Find("ocean").transform.localPosition = new Vector3(17,-30.3f,284);
            //GameObject.Find("ocean").SetActive(false);
        }
    
        // Starts the game on level 1, or the tutorial.
        startMenuCamera.gameObject.SetActive(false);
        if(!isDebugMode)
        {
            gameplayManager.startGame(1);
        }
        player.gameObject.SetActive(true);
        startMenu.SetActive(false);

    }

    public static void resetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
