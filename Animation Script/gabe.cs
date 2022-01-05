using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gabe : MonoBehaviour
{
    private Animator animeGabe;
    public Button gabeButton;
    // Start is called before the first frame update
    void Start()
    {
        animeGabe = GameObject.Find("Gabe").GetComponent<Animator>();
       // gabeButton = GameObject.Find("notPLayButton").GetComponent<Button>();
    }

    // Update is called once per frame
    public void runTheGabe()
    {
        animeGabe.SetBool("gabe",true);
        gabeButton.enabled = false;
    }
    public void stopTheGabe(){
        animeGabe.SetBool("gabe",false);
        gabeButton.enabled = true;
        Debug.Log(gabeButton.enabled);
    }
}
