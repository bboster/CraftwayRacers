using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Jacks : MonoBehaviour
{

    public ArcadeDriving2 Speed;
    public GameObject Player;
    [SerializeField] float CutSpeed; // Player's speed will divide by this number

    // Start is called before the first frame update
    void Start()
    { 
        Speed = GetComponent<ArcadeDriving2>();
        Player.GetComponent<ArcadeDriving2>();
    }

    // Update is called once per frame
    void Update()
    {
        Speed = GetComponent<ArcadeDriving2>();
        Player.GetComponent<ArcadeDriving2>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Jack Jack");
            Player.GetComponent<ArcadeDriving2>().EnginePower = Player.GetComponent<ArcadeDriving2>().EnginePower / CutSpeed;
        }
    }
}
