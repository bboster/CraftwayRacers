using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatchaSpawning : MonoBehaviour
{
    public SpawningSystem spawningSystem;

    private void Start()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "NormalRoad")
        {
            spawningSystem.IsWaitingGatcha = true;
        }

        if(collision.gameObject.tag == "Player")
        {
            spawningSystem.gatchaAmount--;
        }
    }
}
