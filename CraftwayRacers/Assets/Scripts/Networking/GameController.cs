using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameController : NetworkBehaviour
{
    public NetworkVariable<PlayerPosition> playerPositions = new NetworkVariable<PlayerPosition>
        (new PlayerPosition { PlayerPositions = new Vector3[4] }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Awake()
    {
        if(IsHost)
        {
            GameControllerSpawnServerRpc();
        }
    }

    [ServerRpc]
    private void GameControllerSpawnServerRpc()
    {
        gameObject.GetComponent<NetworkObject>().Spawn(true);
    }

    private void Update()
    {
        //Debug.Log(playerPositions.Value.PlayerPositions[0]);
    }

    public struct PlayerPosition : INetworkSerializable
    {
        public Vector3[] PlayerPositions;

        //Serializes PlayerPositions array in inspector.
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref PlayerPositions);
        }
    }
}
