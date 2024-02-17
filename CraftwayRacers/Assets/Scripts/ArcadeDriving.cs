using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class ArcadeDriving : MonoBehaviour
{
    public GameObject CenterOfMass;
    private Rigidbody carRb;
    public PlayerInput PlayerInput;
    public float MaxSuspensionLength = 1f, springForce = 100f, springDamp = 10f;
    private float steerValue =0;

    public Transform[] SpringMountList = new Transform[4];
    private float[] lastVelocity = new float[4];
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
        if (CenterOfMass == null)
        {
            CenterOfMass = GameObject.Find("CoM");
        }
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = CenterOfMass.transform.localPosition;
    }

    void FixedUpdate()
    {
        CarSuspension();
    }
    void CarSuspension()
    {
        int i = 0;
        foreach(Transform springMount in SpringMountList)
        {
            RaycastHit hit;
            Debug.DrawRay(springMount.transform.position, -transform.up, Color.red, MaxSuspensionLength);

            if (Physics.Raycast(springMount.transform.position, -transform.up, out hit, MaxSuspensionLength))
            {
                float compression = (MaxSuspensionLength - hit.distance) / MaxSuspensionLength; //Dividing normalizes the compression no matter how big MSL is. ALWAYS POSITIVE
                float dampeningForce = springDamp * (lastVelocity[i] - compression); //+ value indicates spring rebounding. - value indicates still compressing
                if (lastVelocity[i] - compression > 0)  //Setting dF to 0 because we dont need it anymore 
                {
                    dampeningForce = 0;
                }
                else                                   //OR setting dF to a positive value if it is negative...
                {
                    dampeningForce = -dampeningForce;
                }
                float springForceMagnitude = Mathf.Clamp((springForce * compression+dampeningForce), 0, Mathf.Infinity);

                carRb.AddForceAtPosition(hit.normal * springForceMagnitude, springMount.transform.position);
                lastVelocity[i] = compression; //This works because this is in fixedupdate.

                relativeMovement = -1 * carRb.GetPointVelocity(springMount.transform.position);
                Vector3 tangentForward = -Vector3.Cross(hit.normal, transform.right);
                //Makes a forward vector that is perpendicular to the normal and the right vector "cross product" (google it)
                //Negating the cross product ensures that the resulting tangent vector always points in the direction opposite to the object's right direction
                //For some reason, it points opposite to the blue (z) without the negative
                Vector3 tangentRight = Vector3.Cross(hit.normal, transform.forward);
                //these two tangents will be used to apply force to the car according to if its angled on a surface
                Vector3 forward;
                if (i == 2 || i == 3)
                {
                    forward = tangentForward;
                }
                else
                {
                    forward = (tangentForward + tangentRight * (steerValue)).normalized;
                }
                Debug.DrawLine(springMount.transform.position, springMount.transform.position + forward * 10);


            }


            else
            {
                lastVelocity[i] = 0f; // Reset velocity if not grounded
            }
            i++;
        }
    }
}
