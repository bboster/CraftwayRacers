using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbtackBehavior : MonoBehaviour
{
    public Rigidbody rb;
    public SpawningSystem spawningSystem;
    private ArcadeDriving2 Speed;
   
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
            Speed = collision.gameObject.GetComponent<ArcadeDriving2>();

            if (Speed.Shielded == true)
            {
                    //Speed.Shielded = true;
                    // Speed.Shield.SetActive(false);
                    //StartCoroutine(IFrames());
                    Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("NormalRoad"))
        {
            spawningSystem.IsWaiting = true;
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            rb.isKinematic = true;
        }


        /*IEnumerator IFrames()
        {
            yield return new WaitForSeconds(5);
            Car.Shielded = false;
        }*/
    }
}
