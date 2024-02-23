using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Jacks : MonoBehaviour
{

    public NewDriving Driving;

    // Start is called before the first frame update
    void Start()
    {
        Driving = GetComponent<NewDriving>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Driving.CurrentSpeed = Driving.CurrentSpeed / 2;
        }
    }
}
