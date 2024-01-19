using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameController : NetworkBehaviour
{
    public Vector3[] PlayerPositions;

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
