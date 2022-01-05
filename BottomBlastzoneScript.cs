using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomBlastzoneScript : MonoBehaviour
{
    public Collider player;
    private void OnTriggerEnter(Collider other)
    {
        if(other == player)
        {
            player.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.gameObject.GetComponent<Transform>().localPosition = new Vector3(0, 0, 0);
        }    
    }
}
