using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatchaSpawning : MonoBehaviour
{
    public SpawningSystem spawningSystem;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "NormalRoad")
        {
            spawningSystem.IsWaitingGatcha = true;
            rb.isKinematic = true;
        }
    }
}
