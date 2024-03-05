using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PositionTracker : NetworkBehaviour
{
    private GameController gc;

    private void Start()
    {
        if(IsClient)
        {
            //GetGameControllerClientRpc();
        }
    }

    [ClientRpc]
    private void GetGameControllerClientRpc(NetworkObjectReference target)
    {
        if(target.TryGet(out NetworkObject gc))
        {
            this.gc = gc.GetComponentInParent<GameController>();
        }
    }

    void Update()
    {
        PositionSetterServerRpc();
    }

    [ServerRpc]
    public void PositionSetterServerRpc()
    {
        gc.GetComponent<GameController>().playerPositions.Value.PlayerPositions[0] = transform.position;
    }
}
