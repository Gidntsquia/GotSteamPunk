using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTransition : MonoBehaviour
{
    private Vector3 currentPosition,deckPosition,enginePosition;
    private bool transitionTriggered;
    public GameObject player,mainRoomT,mainRoomNT,sidePanelT,sidePanelNT;
    public float transitionDistance,transitionDuration,playerSpeed,cameraSpeed;
    private BoxCollider deckCollider,engineCollider;
    private string enteredDoor;
    public MeshRenderer[] mr;
    public Animator animeSidePanel,animeMainRoom;
    private CharController playerScript;
    void Start(){
        deckCollider = GameObject.Find("doorEnterEngine").GetComponent<BoxCollider>();
        engineCollider = GameObject.Find("doorEnterDeck").GetComponent<BoxCollider>();
        deckPosition = new Vector3(50f,27f,4f);
        enginePosition = new Vector3(-7f,28f,8.5f);
        //player.GetComponent<CharController>();
               
    }
    void FixedUpdate()
    {
        if(transitionTriggered){
            if(enteredDoor=="doorEnterEngine"){
                //playerScript.canMove= false;
                StartCoroutine(playerTransition(deckCollider,engineCollider));
                StartCoroutine(cameraTransition(enginePosition));

                //switching out the backroom model
                mainRoomT.transform.position = new Vector3(51,1,20);
                mainRoomNT.transform.position = new Vector3(51,-119,20);
                sidePanelT.transform.position = new Vector3(51f,1f,19f);
                sidePanelNT.transform.position = new Vector3(51f,-50f,19f);
                //disabling roof
                mr[0].enabled = false;
                //playing fade animation
                animeSidePanel.SetBool("outside",false);
                animeMainRoom.SetBool("outside",false);

            }
            if(enteredDoor =="doorEnterDeck"){
                //playerScript.canMove= false;
                StartCoroutine(playerTransition(engineCollider,deckCollider));
                StartCoroutine(cameraTransition(deckPosition));
                                
                mainRoomT.transform.position = new Vector3(51,-119,20);
                mainRoomNT.transform.position = new Vector3(51,1,20);
                sidePanelT.transform.position = new Vector3(51f,-50f,19f);
                sidePanelNT.transform.position = new Vector3(51f,1f,19f);
                mr[0].enabled = true;
                animeSidePanel.SetBool("outside",true);
                animeMainRoom.SetBool("outside",true);
            }
        }
        
    }
    private IEnumerator cameraTransition(Vector3 endLocation){
        Camera.main.transform.position =  Vector3.Lerp(Camera.main.transform.position,endLocation,Time.deltaTime*cameraSpeed);
        yield return new WaitForSeconds(transitionDuration);
    }
    private IEnumerator playerTransition(Collider collider,Collider otherDoor){
        collider.isTrigger = false;
        Vector3 transitionDirection = new Vector3(transitionDistance+currentPosition.x, currentPosition.y, currentPosition.z);
        Debug.Log(transitionDirection);
        player.transform.position = Vector3.Lerp(
            player.transform.position, 
            transitionDirection, 
            Time.deltaTime * playerSpeed);

        yield return new WaitForSeconds(transitionDuration);
        
        otherDoor.isTrigger = true;
        transitionTriggered = false;
        playerScript.canMove= true;
    }
    private void OnTriggerEnter(Collider other){
        
        if(other.tag == "Player"){
            Debug.Log("The player has entered "+ gameObject.name);
            transitionTriggered = true;
            enteredDoor = gameObject.name;
            currentPosition = other.gameObject.transform.position;
        }
    }
    
}
