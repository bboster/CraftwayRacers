using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArcadeDriving2 : MonoBehaviour
{
    public GameObject CenterOfMass;
    public Transform[] SpringMountList = new Transform[4];
    public GameObject[] Wheels = new GameObject[4]; 
    public float TopSpeed =20f, MaxSuspensionLength = 2f, SpringStrength=10f, SpringDamper=1f, WheelRadius=0.5f, FrontTireGrip=.6f, RearTireGrip = .3f, TireMass = 1f, EnginePower=10f, MinSteer=30f, MaxSteer=40f, BrakePower = 50f;
    public RaycastHit[] HitList = new RaycastHit[4];
    public AnimationCurve TorqueCurve;
    public Rigidbody CarRb;
    public PlayerInput PlayerInput;
    private bool readingGas, readingBrake;
    private float steerValue = 0, ACValue = 0;
    //public TMP_Text accelText; 
    void Start()
    {
        PlayerInput.currentActionMap.FindAction("Steer").performed += ctx => steerValue = ctx.ReadValue<float>();
        PlayerInput.currentActionMap.FindAction("Steer").canceled += ctx => steerValue = 0;
        PlayerInput.currentActionMap.FindAction("Gas").started += ReadGas;
        PlayerInput.currentActionMap.FindAction("Brake").started += ReadBrake;
        PlayerInput.currentActionMap.FindAction("Gas").canceled += EndReadGas;
        PlayerInput.currentActionMap.FindAction("Brake").canceled += EndReadBrake;

        if (CenterOfMass == null)
        {
            CenterOfMass = GameObject.Find("CoM");
        }
        CarRb = GetComponent<Rigidbody>();
    }
    void ReadGas(InputAction.CallbackContext ctx)
    {
        readingGas = true; 
    }

    void EndReadGas(InputAction.CallbackContext ctx)
    {
        readingGas = false;
    }
    void ReadBrake(InputAction.CallbackContext ctx)
    {
        readingBrake= true;
    }
    void EndReadBrake(InputAction.CallbackContext ctx)
    {
        readingBrake = false;
    }
    void Update()
    {
        if(readingGas == true)
        {
            ACValue = PlayerInput.currentActionMap.FindAction("Gas").ReadValue<float>();
        }
        if (readingBrake == true)
        {
            ACValue = PlayerInput.currentActionMap.FindAction("Brake").ReadValue<float>()*-1f;
        }
        int temp = 0;
        for (int i = temp; i < SpringMountList.Length; i++)
        {
            Suspension(SpringMountList[i], i);
        }
    }
    void FixedUpdate()
    {
        int temp = 0;
        for (int i = temp; i < SpringMountList.Length; i++)
        {
            TractionForce(SpringMountList[i], i);
            DrivingForce(SpringMountList[i], i);
        }
    }
    public bool IsGrounded(Transform springLoc, int springNum)
    {
        Debug.DrawRay(springLoc.position, -transform.up, Color.red, MaxSuspensionLength);
        if (Physics.Raycast(springLoc.position, -transform.up, out HitList[springNum], MaxSuspensionLength))
        {
            return true;
        }
        return false;
    }
    public void DrivingForce(Transform springLoc, int springNum)
    {
        if (IsGrounded(springLoc, springNum))
        {           
            Vector3 accelDir = SpringMountList[springNum].forward;
            float currentSpeed = Vector3.Dot(transform.forward, CarRb.velocity);
            float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(currentSpeed / TopSpeed));
            float availableTorque = TorqueCurve.Evaluate(normalizedSpeed) * ACValue;
            if (currentSpeed > 0 && ACValue < 0)
            {
                //steerValue *= -1f;
            }
            if (springNum == 0 || springNum == 1)
            {
                float steeringFactor = steerValue * MinSteer;
                //float steeringFactor = steerValue * Mathf.Lerp(MinSteer, MaxSteer, normalizedSpeed);
                accelDir += SpringMountList[springNum].right * steeringFactor;
                //ad = new Vector3(accelDir.x*(Mathf.Lerp(MinSteer, MaxSteer, normalizedSpeed)*ACValue), accelDir.y, accelDir.z);
                //print(accelDir += SpringMountList[springNum].right * steeringFactor);
            }
 

            CarRb.AddForceAtPosition(accelDir * (availableTorque * EnginePower), SpringMountList[springNum].transform.position);
            //CarRb.AddForceAtPosition(ad * (availableTorque * EnginePower), SpringMountList[springNum].transform.position);

            if (currentSpeed > 0 && ACValue < 0)
            {
                //Vector3 newAccelDir = new Vector3(accelDir.x * steerValue, accelDir.y, accelDir.z);
                CarRb.AddForceAtPosition(accelDir * BrakePower * ACValue, SpringMountList[springNum].transform.position);
                //print("1" + (accelDir * BrakePower * ACValue, SpringMountList[springNum].transform.position));
                //print("2 " + (newAccelDir * BrakePower * ACValue, SpringMountList[springNum].transform.position));
            }
        }        
    }
    public void TractionForce(Transform springLoc, int springNum)
    {
        if (IsGrounded(springLoc, springNum))
        {
            /*
            Vector3 wheelPos = Wheels[springNum].transform.position;
            wheelPos = new Vector3(HitList[springNum].point.x, HitList[springNum].point.y + WheelRadius, HitList[springNum].point.z);

            Vector3 steeringDir = Wheels[springNum].transform.right;
            Vector3 ContactPoint = HitList[springNum].point;

            Vector3 tireWorldVel = CarRb.GetPointVelocity(wheelPos);
            float steeringValue = Vector3.Dot(steeringDir, tireWorldVel);
            float velChangeByFriction = (-steeringValue * TireGrip) /Time.fixedDeltaTime;

            Debug.DrawLine(wheelPos, wheelPos+steeringDir*10, Color.red);
            //Remember: Force = Mass * acceleration
            CarRb.AddForceAtPosition(TireMass * steeringDir * velChangeByFriction, wheelPos);  
            */
            Vector3 steeringDir = SpringMountList[springNum].right;           
            Vector3 tireVel = CarRb.GetPointVelocity(SpringMountList[springNum].transform.position);
            float steeringVel = Vector3.Dot(steeringDir, tireVel);

            float gripChoice = RearTireGrip;
            if (springNum == 0 || springNum == 1)
            {
                gripChoice = FrontTireGrip;
            }
            float desiredFrictionChange = (-steeringVel * gripChoice)/Time.fixedDeltaTime;
            CarRb.AddForceAtPosition(steeringDir * TireMass * desiredFrictionChange, SpringMountList[springNum].transform.position);
            Debug.DrawRay(SpringMountList[springNum].transform.position, SpringMountList[springNum].transform.position + steeringDir*10f, Color.yellow);
        }
    }
    public void Suspension(Transform springLoc, int springNum)
    {
        if(IsGrounded(springLoc, springNum))
        {
            float compressionOffset = ((MaxSuspensionLength) - HitList[springNum].distance);
            Vector3 springMountVelocity = CarRb.GetPointVelocity(SpringMountList[springNum].position);
            float velocity = Vector3.Dot(SpringMountList[springNum].up, springMountVelocity);
            float dampenedForce = ((compressionOffset * SpringStrength) - velocity * SpringDamper);
            CarRb.AddForceAtPosition(SpringMountList[springNum].up * dampenedForce, SpringMountList[springNum].position);
        }      
    }
}
