using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RelayServerController rsc;
    [SerializeField] private GameObject input;

    public void StartRelayServer()
    {
        rsc.CreateRelay();
    }

    /*public void JoinRelayServer(string code)
    {
        rsc.JoinRelay();
    }*/

    public void ClientPress()
    {
        input.SetActive(true);
    }
}
