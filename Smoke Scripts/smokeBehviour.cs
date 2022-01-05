using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smokeBehviour : MonoBehaviour
{
    private GameObject player;
    private float nextActionTime = 0.0f;
    public float interpolationPeriod = 5f;
    public GameObject smokeParticle;
    private BoxCollider smokeHitBox;
    // Start is called before the first frame update
    void Start()
    {   
        player = GameObject.Find("Sailor");
        
        smokeHitBox = GameObject.Find("smokeHitBox").GetComponent<BoxCollider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        var emission = smokeParticle.GetComponent<ParticleSystem>().emission; 
     if (Time.time > nextActionTime ) {
        nextActionTime += interpolationPeriod;
        if(emission.enabled  == true){
            Debug.Log("fuck");
            emission.enabled = false;
            smokeHitBox.enabled = false;
        }else{
            Debug.Log("me");
            emission.enabled = true;
            smokeHitBox.enabled = true;
        }
         // execute block of code here
     }      
    }
    void onTriggerEnter(Collider other){
        if(other.tag =="player"){
            Debug.Log("the player has lost time");
        }
    }
}
