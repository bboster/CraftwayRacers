using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManagerScript : MonoBehaviour
{
    public Vector3 Player1Spawn;
    public Vector3 Player2Spawn;
    public Vector3 Player3Spawn;
    public Vector3 Player4Spawn;

    public PlayerInput PlayerCount;
    
    public void Start()
    {
        if(PlayerCount.playerIndex == 1)
        {
            transform.position = new Vector3 (Player2Spawn.x, Player2Spawn.y, Player2Spawn.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
