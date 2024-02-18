using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.InputSystem;


public class ArcadeDriving : MonoBehaviour
{
    public GameObject CenterOfMass;
    private Rigidbody carRb;
    public PlayerInput PlayerInput;
    public float CurrentGearRatio=1f, EngineTorque =0.25f, MaxSuspensionLength = 1f, SpringForce = 100f, SpringDamp = 10f, SteeringSensitivity=1f, SurfacesStaticFriction=3f, KineticFriction=0.5f, SlipThreshold = 350f;
    private float steerValue =0, engineRPM, smoothedEngineRPM, weightFeel = 0.0005f, stopWheelSpinFeel = 0.01f, ACValue;

    public Transform[] SpringMountList = new Transform[4];
    private float[] lastVelocity = new float[4];
    private float[] slipAmount = new float[4];
    private float[] wheelRPM= new float[4];
    private Vector3[] relativeMovementList = new Vector3[4];
    private string[] slipPrintout = new string[4];
    private string printout = "Dash: slipping. :";
    public Vector3 relativeMovement;


    void Start()
    {
        /*
        PlayerInput.currentActionMap.FindAction("Accelerate").started += ctx => AccelerateOn();
        PlayerInput.currentActionMap.FindAction("Accelerate").canceled += ctx => AccelerateOff();
        PlayerInput.currentActionMap.FindAction("Reverse").started += ctx => ReverseOn();
        PlayerInput.currentActionMap.FindAction("Reverse").canceled += ctx => ReverseOff();
        PlayerInput.currentActionMap.FindAction("Steer").performed += ctx => steerValue = ctx.ReadValue<float>();
        PlayerInput.currentActionMap.FindAction("Steer").canceled += ctx => steerValue = 0;
        PlayerInput.currentActionMap.FindAction("Flip").performed += ctx => Flip();
        PlayerInput.currentActionMap.FindAction("Rotate").performed += ctx => Rotate();
        */
        PlayerInput.currentActionMap.FindAction("Steer").performed += ctx => steerValue = ctx.ReadValue<float>();
        PlayerInput.currentActionMap.FindAction("Steer").canceled += ctx => steerValue = 0;
        PlayerInput.currentActionMap.FindAction("Test").performed += ctx => ACValue = ctx.ReadValue<float>();
        if (CenterOfMass == null)
        {
            CenterOfMass = GameObject.Find("CoM");
        }
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = CenterOfMass.transform.localPosition;
    }

    void FixedUpdate()
    {
        print(ACValue);
        CarSuspension();
    }
    void CarSuspension()
    {
        int temp = 0;
        for(int i = temp; i < SpringMountList.Length; i++)
        {
            RaycastHit hit;
            Debug.DrawRay(SpringMountList[i].transform.position, -transform.up, Color.red, MaxSuspensionLength);
            if (Physics.Raycast(SpringMountList[i].transform.position, -transform.up, out hit, MaxSuspensionLength))
            {
                //SUSPENSION
                float compression = (MaxSuspensionLength - hit.distance) / MaxSuspensionLength; //Dividing normalizes the compression no matter how big MSL is. ALWAYS POSITIVE
                float dampeningForce = SpringDamp * (lastVelocity[i] - compression); //+ value indicates spring rebounding. - value indicates still compressing
                if (lastVelocity[i] - compression > 0)  //Setting dF to 0 because we dont need it anymore 
                {
                    dampeningForce = 0;
                }
                else                                   //OR setting dF to a positive value if it is negative...
                {
                    dampeningForce = -dampeningForce;
                }
                float springForceMagnitude = Mathf.Clamp((SpringForce * compression + dampeningForce), 0, Mathf.Infinity);

                carRb.AddForceAtPosition(hit.normal * springForceMagnitude, SpringMountList[i].transform.position);
                lastVelocity[i] = compression; //This works because this is in fixedupdate.


                //STEERING
                relativeMovement = -1 * carRb.GetPointVelocity(SpringMountList[i].transform.position);
                Vector3 tangentForward = -Vector3.Cross(hit.normal, transform.right);
                //Makes a forward vector that is perpendicular to the normal and the right vector "cross product" (google it)
                //Negating the cross product ensures that the resulting tangent vector always points in the direction opposite to the object's right direction
                //For some reason, it points opposite to the blue (z) without the negative
                Vector3 tangentRight = Vector3.Cross(hit.normal, transform.forward);
                //these two tangents will be used to apply force to the car according to if its angled on a surface
                Vector3 forward;
                if (i == 2 || i == 3) //Checking if rear wheels
                {
                    forward = tangentForward;
                }
                else
                {
                    forward = (tangentForward + tangentRight * (steerValue * SteeringSensitivity)).normalized;
                }
                Debug.DrawLine(SpringMountList[i].transform.position, SpringMountList[i].transform.position + forward * 10);

                //DRIVING
                float MaxForceB4Slip = springForceMagnitude * SurfacesStaticFriction;
                slipAmount[i] = relativeMovement.magnitude * SlipThreshold / MaxForceB4Slip; // above 1 means sliding
                if (slipAmount[i] < 1.0f)
                {
                    carRb.AddForceAtPosition(relativeMovement * MaxForceB4Slip, hit.point);
                    slipPrintout[i] = "-";
                }
                else
                {
                    carRb.AddForceAtPosition(relativeMovement.normalized * (MaxForceB4Slip / SlipThreshold) * springForceMagnitude * KineticFriction, hit.point);
                    slipPrintout[i] = ".";
                }
            }
            else
            {
                lastVelocity[i] = 0f; // Reset velocity if not grounded
            }

            relativeMovementList[i] = transform.InverseTransformDirection(relativeMovement);
            //Transforming the relative movement vector to local space to calculate wheelRPM, to ultimately apply a force on the wheels in local space

            /*
            for (int j = 0; j < slipPrintout.Length; j++)
            {
                printout += slipPrintout[i];
            }
            print(printout);
            printout= "Dash = slipping:  ";
            */
        }
        smoothedEngineRPM = Mathf.Lerp(smoothedEngineRPM, engineRPM, 0.8f);
        //Each frame, smoothedEngineRPM moves 80% of the way towards engineRPM. A larger fraction means a faster transition.
    }
    void drive()
    {
        for (int i = 0; i < 4; i++)
        {
            //if the wheels are slipping, the rpm of BOTH engine and wheel is affected much less (weightFeel is several times smaller than SpinFeel) 
            if (slipAmount[i] < 1)
            {
                wheelRPM[i] -= (relativeMovementList[2].z + relativeMovementList[3].z) * weightFeel * 10;
                engineRPM -= ((relativeMovementList[2].z + relativeMovementList[3].z) * weightFeel) / 4;
            }
            else
            {
                wheelRPM[i] -= (relativeMovementList[2].z + relativeMovementList[3].z) * stopWheelSpinFeel * 10;
                engineRPM -= ((relativeMovementList[2].z + relativeMovementList[3].z) * stopWheelSpinFeel) / 4;
                //When slip amount is high, (More than 1) this factor is used to adjust the wheel and engine rotations per
                //second to reduce or stop wheel spin, thereby improving traction and control.
            }

            wheelRPM[i] *= (smoothedEngineRPM * CurrentGearRatio);
            //engineRPM += EngineTorque * input.y / (1 + 3 * slipAmount[i]);
        }
    }
}
