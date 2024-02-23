using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbtackBehavior : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "NormalRoad")
        {
            rb.isKinematic = true;
        }
    }
}
