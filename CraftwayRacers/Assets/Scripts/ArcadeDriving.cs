using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArcadeDriving : MonoBehaviour
{
    public GameObject CenterOfMass;
    private Rigidbody carRb;
    public PlayerInput PlayerInput;
    public float MaxSuspensionLength = 1f, springForce = 100f, springDamp = 10f;

    public Transform[] SpringMountList = new Transform[4];
    private float[] lastVelocity = new float[4];


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
            }


            else
            {
                lastVelocity[i] = 0f; // Reset velocity if not grounded
            }
            i++;
        }
    }
}
