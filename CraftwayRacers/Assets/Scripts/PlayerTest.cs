using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

//MAKE SURE ALL OBJECT BEING USED OVER A NETWORK INHERIT FROM NetworkBehaviour INSTEAD OF Monobehaviour
public class PlayerTest : NetworkBehaviour
{
    private int playerID;

    [SerializeField] private float spd;

    private InputActionAsset testActions;
    private InputActionMap testMap;

    private InputAction move;

    private Coroutine moveAction;

    private GameObject gc;

    //Sets up references for movement action.
    private void Awake()
    {
        testMap = GetComponent<PlayerInput>().currentActionMap;

        move = testMap.FindAction("Movement");

        move.started += Move_Started;
        move.canceled += Move_Canceled;

        //Assigns a playerID.
        playerID = GameObject.FindGameObjectsWithTag("Player").Length;
        Debug.Log(playerID);
    }

    /// <summary>
    /// Listener for movement.
    /// </summary>
    /// <param name="context"></param>
    private void Move_Started(InputAction.CallbackContext context)
    {
        if (IsOwner)
        {
            moveAction = StartCoroutine(movement());
        }
    }

    private void Move_Canceled(InputAction.CallbackContext context)
    {
        StopCoroutine(moveAction);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private IEnumerator movement()
    {
        if(gc == null)
        {
            gc = GameObject.FindGameObjectWithTag("GameController");
        }

        for(; ; )
        {
            Vector2 moveDir = move.ReadValue<Vector2>();

            GetComponent<Rigidbody>().velocity = new Vector3(moveDir.x * spd, 0f, moveDir.y * spd);

            //Sets network positions variables according to the ID of the player.
            switch(playerID)
            {
                case 1:
                    gc.GetComponent<GameController>().Player1Pos.Value = transform.position;
                    break;
                case 2:
                    gc.GetComponent<GameController>().Player2Pos.Value = transform.position;
                    break;
                case 3:
                    gc.GetComponent<GameController>().Player3Pos.Value = transform.position;
                    break;
                case 4:
                    gc.GetComponent<GameController>().Player4Pos.Value = transform.position;
                    break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public override void OnDestroy()
    {
        move.started -= Move_Started;
    }
}
