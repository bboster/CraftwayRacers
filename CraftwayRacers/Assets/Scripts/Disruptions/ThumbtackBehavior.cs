using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbtackBehavior : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    public bool isOnGround = false;
   
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "NormalRoad")
        {
            isOnGround = true;
            rb.isKinematic = true;
        }
    }
    
}
