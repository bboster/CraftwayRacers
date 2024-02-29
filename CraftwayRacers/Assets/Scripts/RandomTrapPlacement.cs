using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class RandomTrapPlacement : MonoBehaviour
{
    private bool onCooldown = false;

    [SerializeField] private GameObject cooldownMarker;
    private GameObject spawnManager;

    private void Start()
    {
        cooldownMarker.SetActive(false);
    }

    public void CallSpawn()
    {
        if(spawnManager == null)
        {
            spawnManager = GameObject.FindGameObjectWithTag("SpawnManager");
        }

        spawnManager.GetComponent<NetworkSpawnManager>().CallFunc();
    }

    public void TrapCaller(int index, int chosenTrap)
    {
        if (spawnManager == null)
        {
            spawnManager = GameObject.FindGameObjectWithTag("SpawnManager");
        }

        spawnManager.GetComponent<NetworkSpawnManager>().CallSpawner(index, chosenTrap);
    }
}
