using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSplitter : MonoBehaviour
{
    private int index = 0;

    [SerializeField] private GameObject inputManager;

    public void JoinPlayer()
    {
        if(index < 4)
        {
            PlayerInput p = inputManager.GetComponent<PlayerInputManager>().JoinPlayer(index, index);
            
            p.enabled = false;
            index++;
        }
    }
}
