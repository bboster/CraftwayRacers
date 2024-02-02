using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WASDMovement : MonoBehaviour
{
    public float MoveSpeed;
    public float ReverseSpeed;
    public float TurnSpeed;
    public Transform orientation;
    private float verticalInput;
    private float hInput;
    Vector3 moveDirection;
    Rigidbody rb;

    InputActions controls;
    public float Turn;
    public bool GoPedal = false;
    public bool BackPedal = false;

    public PlayerInput MyInput;

    public InputAction Wheel;
    public InputAction Accelerate;
    public InputAction Reverse;

    private void Awake()
    {
        controls = new InputActions();

        Wheel = MyInput.actions.FindAction("Wheel");
        Accelerate = MyInput.actions.FindAction("Accelerate");
        Reverse = MyInput.actions.FindAction("Reverse");

        Wheel.performed += ctx => Turn = ctx.ReadValue<float>();
        Wheel.canceled += ctx => Turn = 0;

        Accelerate.performed += ctx => GoPedal = true;
        Accelerate.canceled += ctx => GoPedal = false;

        Reverse.performed += ctx => BackPedal = true;
        Reverse.canceled += ctx => BackPedal = false;
    }
    
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Drive();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (GoPedal == true)
        {
            verticalInput = 1;
            Debug.Log("DRIVE");
        }

        if (BackPedal == true)
        {
            verticalInput = -1;
            Debug.Log("REVERSE");
        }

        else
        {
            verticalInput = 0;
        }
    }
    private void Drive()
    {
        moveDirection = orientation.forward * verticalInput;
        orientation.Rotate(0, hInput * TurnSpeed, 0);
        rb.AddForce(moveDirection.normalized * MoveSpeed, ForceMode.Force);
    }
}
