using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkSpawnManager : NetworkBehaviour
{
    private GameObject[] trapPositions;
    private GameObject[] markerPositions;
    private GameObject[] placedTraps;
    [SerializeField] private GameObject[] traps;

    [SerializeField] private GameObject trapTest;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject netParent;

    private void Start()
    {
        gameObject.GetComponent<NetworkObject>().Spawn(true);

        //Add all points to trapPositions
        trapPositions = GameObject.FindGameObjectsWithTag("TPoint");
        markerPositions = GameObject.FindGameObjectsWithTag("PlacementPoint");

        foreach(GameObject i in markerPositions)
        {
            print(i.name);
        }

        placedTraps = new GameObject[markerPositions.Length];

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

        if(placedTraps[choice] == null)
        {
            GameObject local = Instantiate(trapTest, markerPositions[choice].transform.position, Quaternion.identity);
            local.GetComponent<NetworkObject>().Spawn(true);
            placedTraps[choice] = local;

            GameObject obj = Instantiate(trapTest, trapPositions[choice].transform.position, Quaternion.identity);
            obj.GetComponent<NetworkObject>().Spawn(true);
        }
        else
        {
            CallFunc();
        }

            /*GameObject local = Instantiate(trapTest, parent.transform);
            local.GetComponent<NetworkObject>().Spawn(true);
            local.transform.position = trapPositions[choice].transform.localPosition;

            GameObject obj = Instantiate(trapTest, netParent.transform);
            obj.GetComponent<NetworkObject>().Spawn(true);
            obj.transform.localPosition = trapPositions[choice].transform.localPosition;*/
    }

    public void CallSpawner(int index, int chosenTrap)
    {
        SpawnChosenTrapServerRpc(index, chosenTrap);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnChosenTrapServerRpc(int index, int chosenTrap)
    {
        if(placedTraps[index] == null)
        {
            GameObject local = Instantiate(traps[chosenTrap], markerPositions[index].transform.position, Quaternion.identity);
            local.GetComponent<NetworkObject>().Spawn(true);
            placedTraps[index] = local;

            GameObject obj = Instantiate(traps[chosenTrap], trapPositions[index].transform.position, Quaternion.identity);
            obj.GetComponent<NetworkObject>().Spawn(true);
        }
        else
        {
            throw new System.Exception("TRAP ALREADY PLACED HERE.");
        }
    }
}