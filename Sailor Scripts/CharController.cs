using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharController : MonoBehaviour
{
    public float dashForce = 40f;
    public float dashDuration;
    public float walkSpeed = 10f;
    private float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;
    private Vector3 movementVector = Vector3.zero;
    private HookController hookController;
    Vector3 forward, right;
    public bool isHooked = false;
    public bool canMove = true;
    public bool paused = false;
    public bool canPause = false;
    private TextMeshProUGUI tp;
    private Image fade; 
    private HookController hookScript;
    KeyCode dash = KeyCode.LeftShift;


    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        hookScript = gameObject.GetComponent<HookController>();
        fade = GameObject.Find("greyOut").GetComponent<Image>();
        tp = GameObject.Find("pauseText").GetComponent<TextMeshProUGUI>();
        hookController = GetComponent<HookController>();
        rb = GetComponent<Rigidbody>();
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    // Update is called once per frame
    void Update()
    {  
        if (Input.GetKeyDown(KeyCode.Escape) && paused == false && canPause)
        {   
            hookScript.canHook = false;
            canMove = false;
            fade.enabled = true;
            tp.enabled = true;
            Time.timeScale = 0f;
            paused = true;
        }else if(Input.GetKeyDown(KeyCode.Escape) && paused == true && canPause){
            hookScript.canHook = true;
            fade.enabled = false;
            tp.enabled = false;
            Time.timeScale = 1f;
            canMove = true;
            paused = false;
        }

        handleDirection();
        
        if(Input.anyKey){
            HandleMovement();
        }
    }
    void handleDirection()
    {
        isHooked = hookController.isHooked;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movementVector = Vector3.ClampMagnitude(transform.right * horizontal + transform.forward * vertical, 1.0f);
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
    void HandleMovement()
    {   
        //Changes walk and sprint mechanics
        if(Input.GetKeyDown(dash) && isHooked){
            hookController.StopHook();
            StartCoroutine(DashCast());
        }
        //else if(Input.GetKey(dash)){
        //    Move(Input.GetKey(sprint) ? sprintSpeed : walkSpeed, Input.GetKey(sprint))
        //}
        else if(pressingMove() && canMove){
            Move(walkSpeed);           
        }
        

    }
    
    void Move(float moveSpeed){
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal * moveSpeed, rb.velocity.y, vertical * moveSpeed);
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime* horizontal;
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * vertical;
        //sets direction character is facing
        rb.velocity = direction;
    }
    bool pressingMove(){
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }
    public IEnumerator DashCast()
    {
        canMove = false;
        rb.AddForce(transform.forward * dashForce, ForceMode.VelocityChange);
        yield return new WaitForSeconds(dashDuration);
        rb.velocity = Vector3.zero;
        canMove = true;
    }
    public void setCanMove(bool canBeMoved){
        canMove = canBeMoved;
    }
}