using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbtackBehavior : MonoBehaviour
{
    public Rigidbody rb;
    public SpawningSystem spawningSystem;
   
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawningSystem = GameObject.Find("SpawningSystemManager").GetComponent<SpawningSystem>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("NormalRoad"))
        {
            spawningSystem.IsWaiting = true;
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            rb.isKinematic = true;
        }
    }
    
}
