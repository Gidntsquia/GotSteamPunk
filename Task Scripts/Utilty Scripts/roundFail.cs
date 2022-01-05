using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roundFail : MonoBehaviour
{
    private GameObject player;
    private HookController hookScript;
    private CharController charScript;
    private Rigidbody rb;

    private Animator animeFade;
    private Animator animeTimer;
    private Timer timeScript;
    // Start is called before the first frame update
    public void Start(){
        timeScript = GameObject.Find("Task List").GetComponent<Timer>();
        animeFade = GameObject.Find("fadeCircle").GetComponent<Animator>();
        animeTimer = GameObject.Find("roundTimer").GetComponent<Animator>();
        player = GameObject.Find("Sailor");
        hookScript = gameObject.GetComponent<HookController>();
        charScript = gameObject.GetComponent<CharController>();
        rb= player.GetComponent<Rigidbody>();
    }
    public void startFail(){
        timeScript.runningAnim = true;
        Debug.Log("fail animation");
        animeFade.SetBool("hasFailed",true);
        animeFade.SetBool("closed",true);

        rb.velocity = new Vector3(0,0,0);

        
    }
    public void startTimerAnime(){
        player.transform.position = new Vector3 (70.7f,1.25f,8.87f);
        player.transform.eulerAngles = new Vector3 (0f,180f,0f);
        animeTimer.SetBool("timer",true);
    }
    public void endTimerAnime(){
        animeTimer.SetBool("timer",false);
        Debug.Log("opening");
        animeFade.SetBool("closed",false);
    }

    public void endFail(){
        Debug.Log("restarting");
        timeScript.runningAnim = false;
        timeScript.UnPause();
        animeFade.SetBool("hasFailed",false);
        charScript.canMove = true;
        hookScript.canHook = true;
    }
}
