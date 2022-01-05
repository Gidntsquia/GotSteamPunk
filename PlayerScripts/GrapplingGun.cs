using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public audioPlayer audioPlayer;
    public LayerMask whatisGrappleable;
    public Transform gunTip, camera, player;
    private SpringJoint joint;
    public float springForce = 4f;
    public float damperForce = 7f;
    public GameObject hookEnding;
    public GameObject hook;
    public GameObject hookParent;
    public Rigidbody hookrb;
    public GameObject hookTip;
    public float shotSpeed;
    public bool isHooked = false;
    private float startingPlayerSpeed;
    private float startingPlayerMaxSpeed;
    // Start is called before the first frame update
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        hook.GetComponentInChildren<Collider>().enabled = false;
        startingPlayerSpeed = player.gameObject.GetComponent<PlayerMovement>().moveSpeed;
        startingPlayerMaxSpeed = player.gameObject.GetComponent<PlayerMovement>().maxSpeed;


    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            StopAllCoroutines();
            StopGrapple();
        }
    }
    void LateUpdate()
    {
        drawRope();
    }
    void StartGrapple()
    {
        hook.GetComponentInChildren<Collider>().enabled = true;
        hook.transform.parent = null;
        Vector3 direction = -hook.transform.up;
        hookrb.velocity = direction * shotSpeed;
        
        // This adds a max length to the grapple.
        StartCoroutine(waitSecondsThenStopGrapple(22.5f / shotSpeed));
    }
    public void addSpringJoint()
    {
        StopAllCoroutines();
        player.gameObject.GetComponent<PlayerMovement>().maxSpeed = 40;
        player.gameObject.GetComponent<PlayerMovement>().moveSpeed = 6000;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = hook.transform.position;

        float distanceFromPoint = Vector3.Distance(player.position, hook.transform.position);

        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * .25f;

        joint.spring = springForce;
        joint.damper = damperForce;
        joint.massScale = 4.5f;
        lr.positionCount = 2;
    }
    public void StopGrapple()
    {
        player.gameObject.GetComponent<PlayerMovement>().maxSpeed = startingPlayerMaxSpeed;
        player.gameObject.GetComponent<PlayerMovement>().moveSpeed = startingPlayerSpeed;
        hook.GetComponentInChildren<Collider>().enabled = false;
        isHooked = false;
        lr.positionCount = 0;
        Destroy(joint);
        hook.transform.parent = hookParent.transform;
        hook.transform.rotation = transform.rotation; 
        hook.transform.position = hook.transform.parent.transform.position;
        hookrb.velocity = Vector3.zero;
    }

    IEnumerator waitSecondsThenStopGrapple(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StopGrapple();
        
    
        
    }
    void drawRope()
    {
        if(!joint) return;
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, hookEnding.transform.position);
    }

    private void increasePlayerSpeed()
    {
        player.gameObject.GetComponent<PlayerMovement>().maxSpeed = 30;
        player.gameObject.GetComponent<PlayerMovement>().moveSpeed = 8000;
    }

    private void resetPlayerSpeed()
    {
        player.gameObject.GetComponent<PlayerMovement>().maxSpeed = startingPlayerMaxSpeed;
        player.gameObject.GetComponent<PlayerMovement>().moveSpeed = startingPlayerSpeed;
    }
}
