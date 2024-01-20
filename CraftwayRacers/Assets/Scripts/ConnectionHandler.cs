using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class ConnectionHandler : NetworkBehaviour
{
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;

    [SerializeField] private Transform gc;

    private void Awake()
    {
        //Adds listeners to host button to start a host.
        hostBtn.onClick.AddListener(() => { NetworkManager.Singleton.StartHost(); });
        hostBtn.onClick.AddListener(() => { SpawnGameController(); });

        //Adds listeners to client button to start a client.
        clientBtn.onClick.AddListener(() => { NetworkManager.Singleton.StartClient(); });
    }

    //Spawns the GameController as a network object.
    public void SpawnGameController()
    {
        Transform spawnedObj = Instantiate(gc);
        spawnedObj.GetComponent<NetworkObject>().Spawn(true);
    }
}
