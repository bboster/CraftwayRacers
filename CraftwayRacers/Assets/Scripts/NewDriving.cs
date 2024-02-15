using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewDriving : MonoBehaviour
{
    public float TerrainDetectorLength = 2f, SteerSensitivity = 1.0f, MaxSteerAngle = 30f, Power = 50f, CurrentSpeed = 0f, MaxSpeed = 50f, BrakePower = 100f;
    public Vector3 CenterOfMass;
    private float steerValue = 0, ForwardVal = 0f, ReverseVal = 0f;
    private PlayerControls playerControls;
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



        carRb = GetComponent<Rigidbody>();
        //carRb.centerOfMass = CenterOfMass;
        StartCoroutine(CalcSpeed());
        //StartCoroutine(DetectTerrain());

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
    }
    void MovePlayer()
    {
        foreach (Wheel wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = ((ForwardVal + ReverseVal) * Power);
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
        if (CurrentSpeed > 10f && ReverseVal < 0)
        {
            foreach (Wheel wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = (BrakePower * 100);
                //print("BeingApplied" + wheel.wheelCollider.brakeTorque);
            }
        }
        else if (CurrentSpeed < 10f)
        {
            foreach (Wheel wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
                //print("NOTBEINGAPPLIED " + wheel.wheelCollider.brakeTorque);
            }
        }
    }
    void SteerPlayer()
    {
        foreach (Wheel wheel in wheels)
        {
            if (wheel.AxleType == Axle.Front)
            {
                var steerAngle = steerValue * SteerSensitivity * MaxSteerAngle * (1 - (CurrentSpeed / MaxSpeed));
                var finalAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
                //Inverse relationship; as current speed increases, 25 will be multiplied by a smaller decimal to get a smaller angle. (To make it harder to accidently oversteer at high speed)       
                //Lerp is included so that steering isn't instantanious
                wheel.wheelCollider.steerAngle = finalAngle;
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
