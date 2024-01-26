using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDriving : MonoBehaviour //DON'T USE
{
    public PlayerInput MyInput;

    public InputAction Accelerate;
    public InputAction Reverse;
    public InputAction Steer;

    InputAction controls;
    public float move;
    public bool APressed = false;
    public bool BPressed = false;

    Vector3 moveDirection;
    public Transform orientation;
    private float verticalInput;
    public float maxSpeed;
    Rigidbody rb;

    public float TurnSpeed = 1f;
    public float DriveSpeed = 1f;
    public float ReverseSpeed = -1f;
    
    // Start is called before the first frame update
    void Awake()
    {

        Accelerate = MyInput.currentActionMap.FindAction("Accelerate");
        Reverse = MyInput.currentActionMap.FindAction("Reverse");
        Steer = MyInput.currentActionMap.FindAction("Steer");

        Steer.performed += ctx => move = ctx.ReadValue<float>();
        Steer.canceled += ctx => move = 0;

        Accelerate.performed += ctx => APressed = true;
        Accelerate.canceled += ctx => APressed = false;

        Reverse.performed += ctx => BPressed = true;
        Reverse.canceled += ctx => BPressed = false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (APressed == true)
        {
            verticalInput = 1;
            Debug.Log("Go");
        }

        else if (BPressed == true)
        {
            verticalInput = -1;
            //GetComponent<Rigidbody>().velocity = new Vector2(ReverseSpeed, 0);
            Debug.Log("Stop");
        }

        else
        {
            verticalInput = 0;
            //GetComponent<Rigidbody>().velocity = new Vector2(0, 0);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput;

        var force =  DriveSpeed * moveDirection;
        var velocity = rb.velocity + force;
        rb.velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        //orientation.Rotate(0, hInput * TurnSpeed, 0);

        //rb.AddForce(moveDirection.normalized * DriveSpeed, ForceMode.Force);
    }

    private void OnEnable()
    {
      // controls.Driving.Enable();
    }

    private void OnDisable()
    {
        //controls.Driving.Disable();
    }
}
