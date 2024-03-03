using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnCar : MonoBehaviour
{
    public PlayerDriving Steering;

    private float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontalInput = Steering.move;

        Steering.orientation.Rotate(0, horizontalInput * Steering.TurnSpeed, 0);
    }
}
