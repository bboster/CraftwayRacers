using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class NewDriving : MonoBehaviour
{
    public float  TerrainDetectorLength = 2f, SteerSensitivity = 1.0f, MaxSteerAngle = 30f, Power = 50f, CurrentSpeed = 0f, MaxSpeed = 50f, BrakePower = 100f, DownForceVal=5f;
    private float steerValue = 0, ForwardVal = 0f, ReverseVal = 0f;
    private PlayerControls playerControls;
    public GameObject CenterOfMass;
    private Rigidbody carRb;
    public PlayerInput PlayerInput;
    public bool IsOnOffRoad = false, IsOnSlickRoad = false, IsOnNormalRoad = false;

    /// <summary>
    /// We need the enum (named integer) do diffrentiate between front and rear so steering 
    /// can be applied correctly
    /// </summary>
    public enum Axle
    {
        Front, Rear
    }
    public enum WheelSide
    {
        Left, Right
    }
    /// <summary>
    /// A struct (structure) is a custom datatype. It allows you to make a list of wheels.
    /// It NEEDS to be serializeable so that we can see our custom datatype in the inspector.
    /// </summary>
    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axle AxleType;
        public WheelSide SideType;
    }
    public List<Wheel> wheels;
    void Start()
    {
        PlayerInput.currentActionMap.FindAction("Accelerate").started += ctx => AccelerateOn();
        PlayerInput.currentActionMap.FindAction("Accelerate").canceled += ctx => AccelerateOff();
        PlayerInput.currentActionMap.FindAction("Reverse").started += ctx => ReverseOn();
        PlayerInput.currentActionMap.FindAction("Reverse").canceled += ctx => ReverseOff();
        PlayerInput.currentActionMap.FindAction("Steer").performed += ctx => steerValue = ctx.ReadValue<float>();
        PlayerInput.currentActionMap.FindAction("Steer").canceled += ctx => steerValue = 0;
        PlayerInput.currentActionMap.FindAction("Flip").performed += ctx => Flip();
        PlayerInput.currentActionMap.FindAction("Rotate").performed += ctx => Rotate();

        //PlayerInput.currentActionMap.FindAction("Quit").performed += ctx => Quit();


        /*
        playerControls = new PlayerControls();
        playerControls.Movement.Enable();
        playerControls.Movement.Accelerate.started += ctx => AccelerateOn();
        playerControls.Movement.Accelerate.canceled += ctx => AccelerateOff();
        playerControls.Movement.Reverse.started += ctx => ReverseOn();
        playerControls.Movement.Reverse.canceled += ctx => ReverseOff();
        playerControls.Movement.Steer.performed += ctx => steerValue = ctx.ReadValue<float>();
        playerControls.Movement.Steer.canceled += ctx => steerValue = 0;
        */


        if(CenterOfMass == null)
        {
            CenterOfMass = GameObject.Find("CoM");
        }
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = CenterOfMass.transform.localPosition;
        StartCoroutine(CalcSpeed());
        //StartCoroutine(DetectTerrain());

    }
    public void Rotate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
        transform.Rotate(0,90,0);
    }
    public void Flip()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y+5f, transform.position.z);
        transform.Rotate(0,0,180);
    }
    public void ChangeDriveValue(Enum TerrainType)
    {

    }
    IEnumerator DetectTerrain()
    {
        while (true)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.down, out hit, TerrainDetectorLength);
            if (hit.collider.CompareTag("OffRoad"))
            {
                IsOnOffRoad = true;
                print("HERLLO");
            }
            yield return new WaitForSeconds(1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SteerPlayer();
        MovePlayer();
        Brake();
        DetectTerrain();
        AddDownForce();
    }
    void AddDownForce()
    {
        carRb.AddForce(-transform.up*DownForceVal*carRb.velocity.magnitude);
    }
    void MovePlayer()
    {
        foreach (Wheel wheel in wheels)
        {
            if(CurrentSpeed < MaxSpeed)
            {
                wheel.wheelCollider.motorTorque = ((ForwardVal + ReverseVal) * Power);
            }          
        }
    }
    IEnumerator CalcSpeed()
    {
        while (true)
        {
            Vector3 prevPos = transform.position;
            yield return new WaitForFixedUpdate();
            CurrentSpeed = (float)Math.Round((Vector3.Distance(transform.position, prevPos) / Time.deltaTime), 0);
            //print(CurrentSpeed);
        }
    }
    void Brake()
    {
        foreach (Wheel wheel in wheels)
        {
            if(wheel.AxleType== Axle.Rear)
            {
                if (CurrentSpeed > 8f && (ReverseVal+ForwardVal) < 0)
                {
                    wheel.wheelCollider.brakeTorque = (BrakePower * 100);
                    carRb.drag = .5f;
                    //print("BeingApplied" + wheel.wheelCollider.brakeTorque);
                    
                }
                else if (CurrentSpeed < 8f)
                {
                    wheel.wheelCollider.brakeTorque = 0;
                    carRb.drag = 0.05f;
                    //print("NOTBEINGAPPLIED " + wheel.wheelCollider.brakeTorque);

                }
            }
        }    
    }
    void SteerPlayer()
    {
        foreach (Wheel wheel in wheels)
        {
            if (wheel.AxleType == Axle.Front)
            {
                
                //var steerAngle = steerValue * SteerSensitivity * MaxSteerAngle * (1 - (CurrentSpeed / MaxSpeed));
                //var finalAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
                wheel.wheelCollider.steerAngle = steerValue * SteerSensitivity * MaxSteerAngle * (1 - (CurrentSpeed / (MaxSpeed+30)));
                //Inverse relationship; as current speed increases, 25 will be multiplied by a smaller decimal to get a smaller angle. (To make it harder to accidently oversteer at high speed)       
                //Lerp is included so that steering isn't instantanious
                //wheel.wheelCollider.steerAngle = finalAngle;
                //wheel.wheelModel.transform.localEulerAngles = new Vector3(wheel.wheelModel.transform.eulerAngles.x, finalAngle, wheel.wheelModel.transform.eulerAngles.z);
                
            }
        }
        
        foreach (Wheel wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

    private void AccelerateOn()
    {
        ForwardVal = 1;
    }
    private void AccelerateOff()
    {
        ForwardVal = 0;

    }
    private void ReverseOn()
    {
        ReverseVal = -1;
    }
    private void ReverseOff()
    {
        ReverseVal = 0;
    }
    public void Quit()
    {
        Application.Quit();
        //EditorApplication.ExitPlaymode();
    }
}