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

    private IEnumerator Cooldown()
    {
        for(int i = 6; i > 0; i--)
        {
            cooldownMarker.GetComponent<Image>().fillAmount = i/6;

            yield return new WaitForSeconds(0.5f);
        }

        onCooldown = false;
        cooldownMarker.SetActive(false);
    }

    public void TrapCaller(int index)
    {
        if (spawnManager == null)
        {
            spawnManager = GameObject.FindGameObjectWithTag("SpawnManager");
        }

        spawnManager.GetComponent<NetworkSpawnManager>().CallSpawner(index);
    }
}
