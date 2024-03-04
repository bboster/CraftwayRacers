using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

//MAKE SURE ALL OBJECT BEING USED OVER A NETWORK INHERIT FROM NetworkBehaviour INSTEAD OF Monobehaviour
public class PlayerTest : NetworkBehaviour
{
    [SerializeField] private float spd;

    private InputActionAsset testActions;
    private InputActionMap testMap;

    private InputAction move;

    private Coroutine moveAction;

    //Sets up references for movement action.
    private void Awake()
    {
      //  testMap = GetComponent<PlayerInput>().cu;

        move = testMap.FindAction("Movement");

        move.started += Move_Started;
        move.canceled += Move_Canceled;
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
        for(; ; )
        {
            Vector2 moveDir = move.ReadValue<Vector2>();

            GetComponent<Rigidbody>().velocity = new Vector3(moveDir.x * spd, 0f, moveDir.y * spd);

            yield return new WaitForEndOfFrame();
        }
    }

    public override void OnDestroy()
    {
        move.started -= Move_Started;
    }
}
