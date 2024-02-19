using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeDriving2 : MonoBehaviour
{
    public Transform[] SpringMountList = new Transform[4];
    public GameObject[] Wheels = new GameObject[4]; 
    public float MaxSuspensionLength = 1f, SpringStrength=10f, SpringDamper=1f, WheelRadius=0.5f, TireGrip=1f, TireMass = 1f;
    public RaycastHit[] HitList = new RaycastHit[4];
    public AnimationCurve FrictionCurve;
    public Rigidbody CarRb;
    void Start()
    {
        CarRb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        int temp = 0;
        for (int i = temp; i < SpringMountList.Length; i++)
        {

            Suspension(SpringMountList[i], i);
            SteeringForce(SpringMountList[i], i);
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
    public void SteeringForce(Transform springLoc, int springNum)
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
            float desiredFrictionChange = (-steeringVel * TireGrip)/Time.fixedDeltaTime;
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
