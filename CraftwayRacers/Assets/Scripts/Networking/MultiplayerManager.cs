using JetBrains.Annotations;
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
    public Vector3 Player4Spawn;

    public bool Ready = false;
    private bool hasRun = false;

    public PlayerInput PlayerCount;

    [SerializeField] private GameObject parent;

    // Start is called before the first frame update
    void Start() //Tells the players' starting positions 
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            Destroy(parent);
        }

        if(PlayerCount.playerIndex == 0)
        {
            transform.position = new Vector3(Player1Spawn.x, Player1Spawn.y, Player1Spawn.z);
        }

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
