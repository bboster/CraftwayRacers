using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeDriving2 : MonoBehaviour
{
    public Transform[] SpringMountList = new Transform[4];
    public float MaxSuspensionLength = 1f, SpringStrength=10f, SpringDamper=1f;
    public RaycastHit[] HitList = new RaycastHit[4];
    //public AnimationCurve FrictionCurve;
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
            //Steering(SpringMountList[i], i);
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
    public void Steering(Transform springLoc, int springNum)
    {
        /*
        if (IsGrounded(springLoc, springNum))
        {
            Vector3 ContactPoint = HitList[springNum].point;
            GameObject obj = new GameObject("NewObject");
            Transform temp = obj.transform;
            temp.position = ContactPoint; //Putting a vector3 in a transform

            Vector3 steeringDirection = temp.right;
            Vector3 tireWorldVal = CarRb.GetPointVelocity(HitList[springNum].point);
            float steeringValue = Vector3.Dot(steeringDirection, tireWorldVal);


            Debug.DrawLine(ContactPoint, ContactPoint+steeringDirection*10);
            //float velBasedOnGrip = -steeringValue * FrictionCurve;
        }*/
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
