using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuTransition : MonoBehaviour
{
    public GameObject mainCanvas;
    private GameObject timerCanvas;
    private CharController playerScript;
    private Animator cameraAnime;
    private CharController charScript;
    void Start(){
        charScript = GameObject.Find("Sailor").GetComponent<CharController>();
        timerCanvas = GameObject.Find("inGameCanvas");
        timerCanvas.GetComponent<Canvas>().enabled = false;
        playerScript = GameObject.Find("Sailor").GetComponent<CharController>();
        cameraAnime = Camera.main.GetComponent<Animator>();
        playerScript.canMove = false;
        FindObjectOfType<audioPlayer>().PlaySound("titleMusic");

    }
    public void captain(){
        FindObjectOfType<audioPlayer>().spawnCaptain("vc1","titleMusic");
    }
    public void setUI(){
        charScript.canPause = true;
        mainCanvas.GetComponent<Canvas>().enabled = false;
        timerCanvas.GetComponent<Canvas>().enabled = true;
    }
    public void changeBGM(){
        FindObjectOfType<audioPlayer>().PlaySound("backgroundMusic");
        FindObjectOfType<audioPlayer>().stopSound("titleMusic");
    }
    private IEnumerator stopDelay(string name){
        yield return new WaitForSeconds(1f);
        FindObjectOfType<audioPlayer>().stopSound(name);

    }
    private IEnumerator startDelay(string name){
        yield return new WaitForSeconds(1f);
        FindObjectOfType<audioPlayer>().PlaySound(name);
    }
    public void gameStart(){
        cameraAnime.SetBool("start",true);
        FindObjectOfType<audioPlayer>().lowerVolumeContinuous("titleMusic");
    }
    public void turnOnplayerMove(){
        playerScript.canMove = true;

    }
    public void fixCameraPosition(){
        Camera.main.GetComponent<Animator>().enabled = false;
        Camera.main.transform.position = new Vector3(50,36,-9);
    }
}
