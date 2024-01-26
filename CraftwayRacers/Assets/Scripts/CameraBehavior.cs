using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject Player;

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player1");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
