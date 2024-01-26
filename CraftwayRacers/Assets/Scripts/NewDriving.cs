using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDriving : MonoBehaviour
{
    public float SteerSensitivity = 1.0f, Power=50f, AccelerationVal=0;
    private float steerValue = 0;
    private PlayerControls playerControls;

    /// <summary>
    /// We need the enum (named integer) do diffrentiate between front and rear so steering 
    /// can be applied correctly
    /// </summary>
    public enum Axle
    {
        Front,
        Rear
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
        playerControls = new PlayerControls();
        playerControls.Movement.Enable();
        playerControls.Movement.Accelerate.started += ctx => AccelerateOn();
        playerControls.Movement.Accelerate.canceled += ctx => AccelerateOff();
        playerControls.Movement.Reverse.started += ctx => ReverseOn();
        playerControls.Movement.Reverse.canceled += ctx => ReverseOff();
        playerControls.Movement.Steer.performed += ctx => steerValue = ctx.ReadValue<float>();
        playerControls.Movement.Steer.canceled += ctx => steerValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        SteerPlayer();
        MovePlayer();
    }
    void MovePlayer()
    {
        foreach(Wheel wheel in wheels) 
        {
            wheel.wheelCollider.motorTorque= (AccelerationVal * Power);
        }
    }
    void SteerPlayer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.AxleType == Axle.Front)
            {
                var steerAngle = steerValue * SteerSensitivity * 30f;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
                
            }
        }
    }

    private void AccelerateOn()
    {
        AccelerationVal = 1;
        print("HELLO");
    }
    private void AccelerateOff()
    {
        AccelerationVal = 0;

    }
    private void ReverseOn()
    {
        AccelerationVal = -1;
    }
    private void ReverseOff()
    {
        AccelerationVal = 0;
    }
}
