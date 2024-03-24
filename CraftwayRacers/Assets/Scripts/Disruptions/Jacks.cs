using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Jacks : MonoBehaviour
{
    private Rigidbody rb;
    private ArcadeDriving2 Speed;
    [Tooltip("BIGGER NUMBERS MEAN MORE SPEED IS REMOVED")][SerializeField] float CutSpeed; // Player's speed will divide by this number

    // Start is called before the first frame update
   

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" )
        {
            Speed = collision.GetComponent<ArcadeDriving2>();
            if(Speed.Shielded == false)
            {
                Debug.Log("Jack Jack");

                rb = Speed.CarRb;

                rb.velocity = rb.velocity / CutSpeed;
                // Vector3 IncomingForce = rb.velocity;
                //rb.AddForce((IncomingForce * -1) / CutSpeed);
                //Player.GetComponent<ArcadeDriving2>().EnginePower = Player.GetComponent<ArcadeDriving2>().EnginePower / CutSpeed;
            }

            if(Speed.Shielded == true)
            {
                Speed.Shielded = true;
                Speed.Shield.SetActive(false);
                StartCoroutine(IFrames());
                Destroy(gameObject);
            }
        }

        IEnumerator IFrames()
        {
            yield return new WaitForSeconds(5);
            Speed.Shielded = false;
        }
    }
}
