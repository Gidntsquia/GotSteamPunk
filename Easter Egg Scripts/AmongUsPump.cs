using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmongUsPump : MonoBehaviour
{
    public Collider player;
    private void OnTriggerStay(Collider other)
    {
        if(other == player && Input.GetAxis("Interact") >= 0.1f)
        {
            FindObjectOfType<audioPlayer>().GetComponent<audioPlayer>().PlaySoundThenStop("AmongUs");
        }
    }

}
