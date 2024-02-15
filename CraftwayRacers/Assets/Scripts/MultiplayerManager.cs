using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerManager : MonoBehaviour
{
    public Vector3 Player1Spawn;
    public Vector3 Player2Spawn;
    public Vector3 Player3Spawn;
    public Vector4 Player4Spawn;

    public PlayerInput PlayerCount;

    // Start is called before the first frame update
    void Start() //Tells the players' starting positions 
    {
        
        if(PlayerCount.playerIndex == 1)
        {
            transform.position = new Vector3(Player2Spawn.x, Player2Spawn.y, Player2Spawn.z);
        }

        if (PlayerCount.playerIndex == 2)
        {
            transform.position = new Vector3(Player3Spawn.x, Player3Spawn.y, Player3Spawn.z);
        }

        if (PlayerCount.playerIndex == 3)
        {
            transform.position = new Vector3(Player4Spawn.x, Player4Spawn.y, Player4Spawn.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
