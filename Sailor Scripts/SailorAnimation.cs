using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailorAnimation : MonoBehaviour
{
    public Animator anim;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Velocity", rb.velocity.magnitude);
    }
}
