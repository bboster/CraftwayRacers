using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CottonShield : MonoBehaviour
{

    private Material Ball;
    // Start is called before the first frame update
    void Start()
    {
        //Ball = GetComponentInChildren<MeshRenderer>().material; //Give the shield object a script and put this in it. After the player hits a jack, start blinking in and out
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
