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
    public float compMod = 100f, DriftForce= 10f;
    //Tooltips for game devs on driving
    [Tooltip("Used for the torque curve, see below. DO NOT USE FOR JUST 'MORE SPEED'")] public float TopSpeed = 100f;
    [Tooltip("How far the car sits off the ground.")]                                   public float MaxSuspensionLength = 1.35f;
    [Tooltip("How much the car resists gravity due to its own mass ")]                  public float SpringStrength = 95f;
    [Tooltip("How fast springs dissipate energy")]                                      public float SpringDamper = 25f;
    [Tooltip("Adjust where the wheel gameobjects sit due to suspension")]               public float WheelPosMod = -50f;
    [Tooltip("Steering values. Higher speeds mean overall steering gets closer to Min")]public float MinSteer = 1f, MaxSteer = 3f;
    [Tooltip("More speed and faster braking values here!")]                             public float EnginePower = 80f, BrakePower = 70f;
    [Tooltip("VALUE BETWEEN 0 & 1!!! How much wheels go in the sideways/x direction while driving forward")] 
                                                                                        public float FrontTireGrip = .8f, RearTireGrip = .6f;
    [Tooltip("Basically acceleration. Mess with keypoints to change engine behavior")]  public AnimationCurve TorqueCurve;

    //Not necessary for changing car behavior
    public Rigidbody CarRb;
    public GameObject CenterOfMass;
    public PlayerInput PlayerInput;
    private bool readingGas, readingBrake, isDrifting=false, canDrift=true;
    private float steerValue = 0, ACValue = 0;
    //Shield Tings
    public bool Shielded;
    public GameObject Shield;
    public float ShieldTimer;

    //SFX bools
    private bool playingBrake = false;

    void Start()
    {
            StartCountdown.StartRace += Handle_StartRace;
        
            Application.targetFrameRate = 120;
        if (CenterOfMass == null)
        {
            CenterOfMass = GameObject.Find("CoM");
        }
        CarRb = GetComponent<Rigidbody>();
        CarRb.centerOfMass = new Vector3(0, -1, 0.125f);

        if(GameObject.Find("SoundManager")!=null)
        {
            StartCoroutine(GameObject.Find("SoundManager").GetComponent<SoundManager>().EngineStart("CarStartSound", gameObject));
        }
    }

    void Handle_StartRace()
    {
        PlayerInput.currentActionMap.FindAction("Steer").performed += ctx => steerValue = ctx.ReadValue<float>();
        PlayerInput.currentActionMap.FindAction("Steer").canceled += ctx => steerValue = 0;
        PlayerInput.currentActionMap.FindAction("Gas").started += ReadGas;
        PlayerInput.currentActionMap.FindAction("Gas").canceled += EndReadGas;
        PlayerInput.currentActionMap.FindAction("Brake").started += ReadBrake;
        PlayerInput.currentActionMap.FindAction("Brake").canceled += EndReadBrake;
        PlayerInput.currentActionMap.FindAction("Drift").started += ReadDrift;
        PlayerInput.currentActionMap.FindAction("Drift").canceled += EndReadDrift;
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

        if(playingBrake == false)
        {
            //SoundManager.instance.Play("BrakingSound", 100);
            playingBrake = true;
        }
    }
    void EndReadBrake(InputAction.CallbackContext ctx)
    {
        readingBrake = false;

        if(playingBrake == true)
        {
            //SoundManager.instance.Stop("BrakingSound");
            playingBrake = false;
        }
    }
    void ReadDrift(InputAction.CallbackContext ctx)
    {
        if (canDrift)
        {
            isDrifting = true;
            MinSteer = .1f;
            MaxSteer = 1f;
            FrontTireGrip = 0.5f;
            RearTireGrip = 0.4f;
        }
    }
    void EndReadDrift(InputAction.CallbackContext ctx)
    {
        isDrifting= false;
        FrontTireGrip = 0.8f;
        RearTireGrip = 0.6f;
        MinSteer = 1f;
        MaxSteer = 3f;
    }
    /// <summary>
    /// Suspension is here because it doesnt work as well/at all in fixed update. Probably something
    /// to do with framerate.
    /// </summary>
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            Handle_StartRace();
        }
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
            ////if either of the front two wheels are touching offroad
            //if (HitList[0].collider.gameObject.layer == LayerMask.NameToLayer("OffRoad") || HitList[1].collider.gameObject.layer == LayerMask.NameToLayer("OffRoad"))
            //{
            //    canDrift = false;
            //    EnginePower = 20f;
            //    TopSpeed = 40f;
            //    FrontTireGrip = 0.9f;
            //    RearTireGrip = 0.7f;
            //    MinSteer = 1.5f;
            //    MaxSteer = 4.5f;
            //}
            ////Else if either of the front two wheels are touching slick
            //else if (HitList[0].collider.gameObject.layer == LayerMask.NameToLayer("Slick") || HitList[1].collider.gameObject.layer == LayerMask.NameToLayer("Slick"))
            //{
            //    canDrift = false;
            //    EnginePower = 20f;
            //    TopSpeed = 40f;
            //    FrontTireGrip = 0.6f;
            //    RearTireGrip = 0.5f;
            //    MinSteer = .25f;
            //    MaxSteer = 2f;
            //}
            ////else if on normal ground, normal values
            //else if (HitList[0].collider.gameObject.layer == LayerMask.NameToLayer("Normal") || HitList[1].collider.gameObject.layer == LayerMask.NameToLayer("Normal"))
            //{
            //    canDrift = true;
            //    EnginePower = 80f;
            //    TopSpeed = 100f;
            //    FrontTireGrip = 0.8f;
            //    RearTireGrip = 0.6f;
            //    MinSteer = 1f;
            //    MaxSteer = 3f;
            //}
            return true;
        }
        return false;
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
            if (currentSpeed > 0 && ACValue < 0)
            {
                steerValue *= -1f;
            }
            if (springNum == 0 || springNum == 1) //STEERING HERE
            {
                float steeringFactor = steerValue * Mathf.Lerp(MaxSteer, MinSteer, normalizedSpeed);
                //As you speed up, you get less steering for improved handling. (Think how little steering it takes to switch lanes on the highway)
                accelDir += SpringMountList[springNum].right * steeringFactor;
                WheelList[springNum].transform.localRotation = Quaternion.Euler(0f, steeringFactor*8f, 0f); //JUST MAKES THE WHEEL GAMEOBJECTS "TURN" visually
            }
 
            //MAIN ACCELERATION HERE
            CarRb.AddForceAtPosition(accelDir * ((availableTorque * EnginePower)+1f), SpringMountList[springNum].transform.position);

            if (currentSpeed > 0 && ACValue < 0) //braking
            {
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
            CarRb.AddForceAtPosition(steeringDir * 1 * desiredFrictionChange, SpringMountList[springNum].transform.position);
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Wall"))
        {
            SoundManager.instance.Play("CarCollisionSound", 100);
        }
    }
    private void OnDestroy()
    {
        PlayerInput.currentActionMap.FindAction("Steer").performed -= ctx => steerValue = ctx.ReadValue<float>();
        PlayerInput.currentActionMap.FindAction("Steer").canceled -= ctx => steerValue = 0;
        PlayerInput.currentActionMap.FindAction("Gas").started -= ReadGas;
        PlayerInput.currentActionMap.FindAction("Gas").canceled -= EndReadGas;
        PlayerInput.currentActionMap.FindAction("Brake").started -= ReadBrake;
        PlayerInput.currentActionMap.FindAction("Brake").canceled -= EndReadBrake;
        PlayerInput.currentActionMap.FindAction("Drift").started -= ReadDrift;
        PlayerInput.currentActionMap.FindAction("Drift").canceled -= EndReadDrift;
    }
}
