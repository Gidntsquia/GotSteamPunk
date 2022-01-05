using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskIndicatorAnimationScript : MonoBehaviour
{
    private Animator animator;
    private IndividualTaskScript parentTask;
    public float animationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        parentTask = this.GetComponentInParent<IndividualTaskScript>();

        animator.speed = 1;
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        animationSpeed = (Mathf.Clamp(parentTask.secondsEnabled, 0, parentTask.secondsEnabled) / parentTask.secondsToComplete) * 5 + 1;  
        animator.speed = animationSpeed;  
    }
}
