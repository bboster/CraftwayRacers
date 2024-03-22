using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;

public class ArcadeDriving2 : MonoBehaviour
{
    //MAIN: https://www.youtube.com/watch?v=CdPYlj5uZeI
    //ALSO: https://www.youtube.com/watch?v=LG1CtlFRmpU&t=418s
    //DEPTH: https://docs.google.com/document/d/1nZgDmK4cqbVv-hbadsT_SPU5ioestrdM_njq1XrQqbo/edit

    //Place to put all of our components. Order DOES matter (0: FrontLeft, 1: => FR, 2: =>RL, 3: =>RR)
    public RaycastHit[] HitList = new RaycastHit[4];
    public Transform[] SpringMountList = new Transform[4];
    public GameObject[] WheelList = new GameObject[4];
    public float compMod = 10f, DriftForce= 10f;
    //Tooltips for game devs on driving
    [Tooltip("Used for the torque curve, see below. DO NOT USE FOR JUST 'MORE SPEED'")] public float TopSpeed = 20f;
    [Tooltip("How far the car sits off the ground.")]                                   public float MaxSuspensionLength = 2f;
    [Tooltip("How much the car resists gravity due to its own mass ")]                  public float SpringStrength = 10f;
    [Tooltip("How fast springs dissipate energy")]                                      public float SpringDamper = 1f;
    [Tooltip("Adjust where the wheel gameobjects sit due to suspension")]               public float WheelPosMod = 0.5f;
    [Tooltip("Steering values. Higher speeds mean overall steering gets closer to Min")]public float MinSteer = 2.5f, MaxSteer = 0.5f;
    [Tooltip("More speed and faster braking values here!")]                             public float EnginePower = 10f, BrakePower = 50f;
    [Tooltip("VALUE BETWEEN 0 & 1!!! How much wheels go in the sideways/x direction while driving forward")] 
                                                                                        public float FrontTireGrip = .6f, RearTireGrip = .3f;
    [Tooltip("Basically acceleration. Mess with keypoints to change engine behavior")]  public AnimationCurve TorqueCurve;

    //Not necessary for changing car behavior
    public Rigidbody CarRb;
    public GameObject CenterOfMass;
    public PlayerInput PlayerInput;
    private bool readingGas, readingBrake, isDrifting;
    private float steerValue = 0, ACValue = 0, WheelRadius = 0.5f, TireMass = 1f;
    
    //Shield Tings
    public bool Shielded;
    public GameObject Shield;
    public float ShieldTimer;

    void Start()
    {
        Application.targetFrameRate = 120;
        PlayerInput.currentActionMap.FindAction("Steer").performed += ctx => steerValue = ctx.ReadValue<float>();
        PlayerInput.currentActionMap.FindAction("Steer").canceled += ctx => steerValue = 0;
        PlayerInput.currentActionMap.FindAction("Gas").started += ReadGas;
        PlayerInput.currentActionMap.FindAction("Gas").canceled += EndReadGas;
        PlayerInput.currentActionMap.FindAction("Brake").started += ReadBrake;
        PlayerInput.currentActionMap.FindAction("Brake").canceled += EndReadBrake;
        PlayerInput.currentActionMap.FindAction("Drift").started += ReadDrift;
        PlayerInput.currentActionMap.FindAction("Drift").canceled += EndReadDrift;

        if (CenterOfMass == null)
        {
            CenterOfMass = GameObject.Find("CoM");
        }
        CarRb = GetComponent<Rigidbody>();
        CarRb.centerOfMass = new Vector3(0, -1, 0.125f);
    }

    /// <summary>
    /// Basically inputs here, nothing important. Changes bools
    /// </summary>
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
    void ReadDrift(InputAction.CallbackContext ctx)
    {
        isDrifting = true;
    }
    void EndReadDrift(InputAction.CallbackContext ctx)
    {
        isDrifting= false;

    }


    /// <summary>
    /// Suspension is here because it doesnt work as well/at all in fixed update. Probably something
    /// to do with framerate.
    /// </summary>
    void Update()
    {
        if (readingGas)
        {
            ACValue = PlayerInput.currentActionMap.FindAction("Gas").ReadValue<float>();
        }
        if(!readingGas)
        {
            ACValue = 0;
        }
        if (readingBrake == true)
        {
            ACValue = PlayerInput.currentActionMap.FindAction("Brake").ReadValue<float>()*-1f;
        }
        for (int i = 0; i < SpringMountList.Length; i++)
        {
            Suspension(SpringMountList[i], i);        
        }
    }
    /// <summary>
    /// Every fixedUpdate/physics step, the script goes through each springmount and determines
    /// what forces to apply to the cars rigidbody by calling these functions. SpringMountList 
    /// contains TRANSFORMS so we can determine vectors and stuff, so we also need to pass along
    /// which spring we are looking at as the second parameter.
    /// </summary>
    void FixedUpdate()
    {
        int temp = 0;
        for (int i = temp; i < SpringMountList.Length; i++)
        {
            TractionForce(SpringMountList[i], i);
            DrivingForce(SpringMountList[i], i);
        }
    }
    /// <summary>
    /// Sends out a raycast from the springmounts (check the gameobjects, attached to chassis) in 
    /// the downwards direction (as far as MaxSusLen allows), and stores the hit data in a hitlist.
    /// </summary>
    /// <param name="springLoc"></param>
    /// <param name="springNum"></param>
    /// <returns></returns>
    public bool IsGrounded(Transform springLoc, int springNum)
    {
        Debug.DrawRay(springLoc.position, -transform.up, Color.red, MaxSuspensionLength);
        if (Physics.Raycast(springLoc.position, -transform.up, out HitList[springNum], MaxSuspensionLength))
        {
            return true;
        }
        return false;
    }
    
    IEnumerator AddDrift()
    {
        yield return null;
    }


    /// <summary>
    /// This where we actually "drive" the car forward and sideways, and reverse if conditions are 
    /// right. We use the lookup curve to determine how much force to apply based on how fast 
    /// the car is already going. (If the number was fixed, acceleration would be fixed and you
    /// would accelerate forever.) CURRENTLY NEEDS WORK, THE CAR DOESNT STEER IF YOU DONT APPLY GAS
    /// </summary>
    public void DrivingForce(Transform springLoc, int springNum)
    {
        if (IsGrounded(springLoc, springNum))
        {           
            Vector3 accelDir = SpringMountList[springNum].forward;
            float currentSpeed = Vector3.Dot(transform.forward, CarRb.velocity);
            float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(currentSpeed / TopSpeed));
            float availableTorque = TorqueCurve.Evaluate(normalizedSpeed) * ACValue;
            if (isDrifting)
            {
                accelDir = SpringMountList[springNum].forward * 2;
                MinSteer = .1f;
                MaxSteer = 1f;
                FrontTireGrip = 0.5f;
                RearTireGrip = 0.4f;
                //CarRb.constraints = RigidbodyConstraints.FreezeRotationY;
                float driftForce = DriftForce; 
                if(steerValue > 0)
                {
                    accelDir -= SpringMountList[springNum].right * (driftForce*normalizedSpeed) ;
                }
                else
                {
                    accelDir += SpringMountList[springNum].right * (driftForce * normalizedSpeed);
                }        
            }
            if(!isDrifting)
            {
                //CarRb.constraints = RigidbodyConstraints.None;
                FrontTireGrip = 0.8f;
                RearTireGrip = 0.6f;
                MinSteer = .5f;
                MaxSteer = 2.5f;
                //Need a coroutine task for a small duration, apply force in the forward direction of the car, at the normal of the ground// might not have to cause suspension?
            }
            if (currentSpeed > 0 && ACValue < 0)
            {
                steerValue *= -1f;
            }
            if (springNum == 0 || springNum == 1) //STEERING HERE
            {
                float steeringFactor = steerValue * Mathf.Lerp(MaxSteer, MinSteer, normalizedSpeed);
                //As you speed up, you get less steering for improved handling. (Think how little steering it takes to switch lanes on the highway)
                accelDir += SpringMountList[springNum].right * steeringFactor;
                //ad = new Vector3(accelDir.x*(Mathf.Lerp(MinSteer, MaxSteer, normalizedSpeed)*ACValue), accelDir.y, accelDir.z);
                //print(accelDir += SpringMountList[springNum].right * steeringFactor);
                WheelList[springNum].transform.localRotation = Quaternion.Euler(0f, steeringFactor*4f, 0f); //JUST MAKES THE WHEEL GAMEOBJECTS "TURN" visually
            }
 
            //MAIN ACCELERATION HERE
            CarRb.AddForceAtPosition(accelDir * ((availableTorque * EnginePower)+1f), SpringMountList[springNum].transform.position);
            //CarRb.AddForceAtPosition(ad * (availableTorque * EnginePower), SpringMountList[springNum].transform.position);

            if (currentSpeed > 0 && ACValue < 0) //braking
            {
                //Vector3 newAccelDir = new Vector3(accelDir.x * steerValue, accelDir.y, accelDir.z);
                CarRb.AddForceAtPosition((accelDir) * BrakePower * ACValue, SpringMountList[springNum].transform.position);
            }
        }        
    }
    /// <summary>
    /// This is all about sideways friction. Without this, there is absolutely ZERO friction (try commenting
    /// out calling this function in FixedUpdate.) It applies force to wheels in the OPPOSITE direction they 
    /// want to go during a turn. (I.e., when turning left, the wheels still want to go forward as well as 
    /// left, and this force makes them go more left than forward based on how high grip is. Notice the if
    /// statement seperating front and rear wheels, having two different friction values is HUGE. It just
    /// makes the car handle corners better if the rear wheels slip more, not quite sure how but its all 
    /// physics stuff.
    /// </summary>
    public void TractionForce(Transform springLoc, int springNum)
    {
        if (IsGrounded(springLoc, springNum))
        {
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
            //Debug.DrawRay(SpringMountList[springNum].transform.position, SpringMountList[springNum].transform.position + steeringDir*10f, Color.yellow);
        }
    }
    /// <summary>
    /// The first half of this script determines the force applied back upwards to make a "suspension". 
    /// Dot product is hard to explain, but think of it as a car hitting a bump at high speed vs low speed.
    /// Because GetPointVelocity returns vectors in all direction components, the 'faster' the 'car' is 
    /// going, the higher "velocity" float will be as a result of hitting a bump. Tbh, dampenedForce 
    /// formula is confusing me at this time but it is basically the final vector in conjunction with 
    /// the dampening force of the spring. When compressing, dampenedForce is negative or a decimal, 
    /// resulting in less force pushing the rigidbody back up during AddForceAtPos.
    /// </summary>
    public void Suspension(Transform springLoc, int springNum)
    {
        string temp = "";
        if(IsGrounded(springLoc, springNum))
        {
            float compressionOffset = ((MaxSuspensionLength) - HitList[springNum].distance);
            Vector3 springMountVelocity = CarRb.GetPointVelocity(SpringMountList[springNum].position);
            float velocity = Vector3.Dot(SpringMountList[springNum].up, springMountVelocity);
            float dampenedForce = ((compressionOffset * SpringStrength) - velocity * SpringDamper);
            CarRb.AddForceAtPosition(SpringMountList[springNum].up * dampenedForce, SpringMountList[springNum].position);


            //WheelList[springNum].transform.position.y = SpringMountList[springNum].position.y - compressionOffset;
            //Vector3 wheelPosition = WheelList[springNum].transform.position;
            //wheelPosition.y = SpringMountList[springNum].position.y + WheelPosMod - (compressionOffset*compMod);
            //WheelList[springNum].transform.position = wheelPosition;
            Vector3 wheelPosition = WheelList[springNum].transform.position;
            wheelPosition.y = SpringMountList[springNum].localPosition.y+WheelPosMod + (compressionOffset*compMod);
            temp += springNum + "Pos: " + wheelPosition.y + "," + "Compression: " + compressionOffset;
            WheelList[springNum].transform.localPosition = new Vector3(WheelList[springNum].GetComponent<WheelInfo>().StartXPosition,
                 wheelPosition.y,
                 WheelList[springNum].GetComponent<WheelInfo>().StartZPosition);
                   
        }
        //print(temp);
    }



    /// <summary>
    /// Caleb's shtuff; will probably be moved to shield script later
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Shield")
        {
            StartCoroutine(waiter());
            Shielded = true;
            Shield.SetActive(true);
        }

        IEnumerator waiter()
        {
            yield return new WaitForSeconds(ShieldTimer);
            Shielded = false;
            Shield.SetActive(false);
        }
    }

}
