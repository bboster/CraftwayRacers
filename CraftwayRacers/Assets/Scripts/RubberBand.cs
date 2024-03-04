using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberBand : MonoBehaviour
{
    private Rigidbody rb;
    private ArcadeDriving2 Speed;
    [SerializeField] public float AddSpeed; // Player's speed will divide by this number

    // Start is called before the first frame update


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Speed = collision.GetComponent<ArcadeDriving2>();
            if (Speed.Shielded == false)
            {
                Debug.Log("Jack Jack");

                rb = Speed.CarRb;

                rb.velocity = (rb.velocity * AddSpeed);
                // Vector3 IncomingForce = rb.velocity;
                //rb.AddForce((IncomingForce * -1) / CutSpeed);
                //Player.GetComponent<ArcadeDriving2>().EnginePower = Player.GetComponent<ArcadeDriving2>().EnginePower / CutSpeed;
            }
        }
    }
}
