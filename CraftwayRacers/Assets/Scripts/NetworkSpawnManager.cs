using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkSpawnManager : NetworkBehaviour
{
    private GameObject[] trapPositions;

    [SerializeField] private GameObject trapTest;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject netParent;

    private void Start()
    {
        gameObject.GetComponent<NetworkObject>().Spawn(true);

        //Add all points to trapPositions
        trapPositions = GameObject.FindGameObjectsWithTag("TPoint");

        parent = GameObject.FindGameObjectWithTag("LocalMap");
        netParent = GameObject.FindGameObjectWithTag("MobileMap");
    }

    public void CallFunc()
    {
        SpawnTrapServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnTrapServerRpc()
    {
            int choice = Random.Range(0, trapPositions.Length);

            GameObject local = Instantiate(trapTest, trapPositions[choice].transform.position, Quaternion.identity);
            local.GetComponent<NetworkObject>().Spawn(true);
            //local.transform.position = trapPositions[choice].position;

            GameObject obj = Instantiate(trapTest, netParent.transform);
            obj.GetComponent<NetworkObject>().Spawn(true);
            obj.transform.localPosition = trapPositions[choice].transform.localPosition;
    }
}
