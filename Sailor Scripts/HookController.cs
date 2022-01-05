using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    public float maxRayDist = 20;
    public LineRenderer lr;
    public Transform hookOrigin, player;
    public bool isCollidedPress;
    public bool isHooked;
    public bool canHook = true;
    public GameObject currentHooked;
    private Vector3 center;
    private Vector3 axis = Vector3.up;
    private float radius;
    public float rotationSpeed = 4.0f;
    private Vector3 hookPoint;
    public LayerMask isHookable;
    public float walkHeight = 1.25f;
    public float unhookVelocity = 5f;
    private Rigidbody rb;
    private CharController characterController;
    private BoxCollider playerCollider;
    private bool isClockWise;
    private float hookDirectionModifier;
    private float realtiveAngle;
    private float quateriony;
    private bool isDeterminingRotation;
    private bool isRotating = false;
    private float clockwiseRotationSpeed;
    private float counterRotationSpeed;
    
    /*

    */

    //TODO: set a ternary operator to set the float to 90 or 270 which determines 
    //the direction: 90 is clockwise, 270 is counterclockwise
    //the rotationspeed also has to be changed to negative to go counter clockwise

       // Start is called before the first frame update
    void Start()
    {
        clockwiseRotationSpeed = rotationSpeed;
        counterRotationSpeed = -rotationSpeed; 
        quateriony = 90;
        playerCollider = GetComponent<BoxCollider>();
        characterController = this.GetComponent<CharController>();
        
        rb = GetComponent<Rigidbody>();
        lr.positionCount = 0;
        isCollidedPress = false;
        lr = GetComponent<LineRenderer>();

    }
    void OnCollisionEnter(Collision collision){
        if(collision.collider.gameObject.layer != 7){
            StopHook();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(canHook){
            if(Input.GetMouseButtonDown(0)){
            StartHook();
            }
        
            else if(Input.GetMouseButtonUp(0) && isHooked || isCollidedPress ){
              isCollidedPress = false;
                StopHook();

            }
            else if(isRotating){
                CheckHook();
         }
        }
        if(Input.GetMouseButtonUp(0)&&isHooked){
            StopHook();
        }

    }
    void FixedUpdate()
    {

        if(isRotating)
        {
            rotationSpeed = isClockWise ? clockwiseRotationSpeed : counterRotationSpeed;
            quateriony = isClockWise ? 90 : 270;
            center = hookPoint;
            transform.RotateAround(center, axis, rotationSpeed);
            Vector3 tangentVector = Quaternion.Euler(0,quateriony,0) * (transform.position - center);
            transform.rotation = Quaternion.LookRotation(tangentVector);
            Vector3 fixedCenterPosition = new Vector3(center.x, walkHeight, center.z);
            
        }
        else if(isDeterminingRotation)
        {
            determineHookDirection();
        }
    }
    void LateUpdate(){
        if(isRotating || isDeterminingRotation){
            DrawHook(hookOrigin.position, hookPoint);
        }
    }
    void StartHook() {
        characterController.canMove = false;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit);
        Vector3 modRayCastHit = new Vector3(raycastHit.point.x, hookOrigin.position.y, raycastHit.point.z);
        Vector3 direction = modRayCastHit - hookOrigin.position;
        if(Physics.Raycast(hookOrigin.position, direction, out hit, maxRayDist, isHookable)){
            isHooked = true;
            isDeterminingRotation = true;
            currentHooked = hit.collider.gameObject;
            hookPoint = hit.point;
            float distanceFromPoint = Vector2.Distance(hookOrigin.position, hookPoint);
            lr.positionCount = 2;
            radius = Vector3.Distance(center, transform.position);

            determineHookDirection();
        }
        else
        {
            characterController.canMove = true;
        }
        
    }
    public void StopHook(){
        isDeterminingRotation = false;
        characterController.canMove = true;
        isHooked = false;
        if(isRotating)
        {
            rb.AddForce(transform.forward * unhookVelocity, ForceMode.VelocityChange);
        }
        isRotating = false;
        lr.positionCount = 0;
    }
    void DrawHook(Vector3 pos1, Vector3 pos2){
        lr.SetPosition(0, pos1);
        lr.SetPosition(1, pos2);
    }
    void CheckHook(){
        RaycastHit hit;
        Vector3 direction = currentHooked.transform.position - hookOrigin.position;
        if(Physics.Raycast(hookOrigin.position, direction, out hit, isHookable)){
            if(hit.collider.gameObject.layer != 7 && hit.collider.gameObject != currentHooked){//hit.collider.gameObject != currentHooked && 
                isCollidedPress = true;
            }
        }
    }

    void determineHookDirection()
    {
        /*
            use the angle to modify values
        */
        realtiveAngle = Mathf.Atan2(transform.position.x - hookPoint.x,
            transform.position.z - hookPoint.z)
             * Mathf.Rad2Deg;
        if(0 < realtiveAngle && realtiveAngle < 90)
        {
            if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                isClockWise = true;
                isDeterminingRotation = false;
                isRotating = true;

            } 
            else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W))
            {
                isClockWise = false;
                isDeterminingRotation = false;
                isRotating = true;
            }
        }
        else if(90 < realtiveAngle && realtiveAngle < 180)
        {
            if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S))
            {
                isClockWise = true;
                isDeterminingRotation = false;
                isRotating = true;
            }
            else if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D))
            {
                isClockWise = false;
                isDeterminingRotation = false;
                isRotating = true;
            }
        }
        else if(-180 < realtiveAngle && realtiveAngle < -90)
        {
            if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W))
            {
                isClockWise = true;
                isDeterminingRotation = false;
                isRotating = true;
            }
            else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                isClockWise = false;
                isDeterminingRotation = false;
                isRotating = true;
            }
        }
        else if(-90 < realtiveAngle && realtiveAngle < 0)
        {
            if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W))
            {
                isClockWise = true;
                isDeterminingRotation = false;
                isRotating = true;
            }
            else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S))
            {
                isClockWise = false;
                isDeterminingRotation = false;
                isRotating = true;
            }
        }
        else{
            Debug.Log("we fucked up we gotta go bald");
        }

    }
    
}