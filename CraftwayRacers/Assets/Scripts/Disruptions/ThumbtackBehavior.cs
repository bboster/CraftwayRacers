using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbtackBehavior : MonoBehaviour
{
    public Rigidbody rb;
    public SpawningSystem spawningSystem;
    private ArcadeDriving2 Car;
   
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawningSystem = GameObject.Find("SpawningSystemManager").GetComponent<SpawningSystem>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Car = collision.GetComponent<ArcadeDriving2>();

            if (Car.Shielded == true)
            {
                    Car.Shielded = true;
                    Car.Shield.SetActive(false);
                    StartCoroutine(IFrames());
                    Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("NormalRoad"))
        {
            spawningSystem.IsWaiting = true;
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            rb.isKinematic = true;
        }


        IEnumerator IFrames()
        {
            yield return new WaitForSeconds(5);
            Car.Shielded = false;
        }
    }
}
