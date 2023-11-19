using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class PlayerTest : MonoBehaviour
{
    private InputActionAsset testActions;
    private InputActionMap testMap;

    private InputAction move;

    private void Awake()
    {
        move = testMap.FindAction("Movement");

        move.started += Move_Started;
    }

    private void Move_Started(InputAction.CallbackContext context)
    {
        Vector2 moveDir = move.ReadValue<Vector2>();

        throw new NotImplementedException();
    }

    private void OnDestroy()
    {
        move.started -= Move_Started;
    }
}
