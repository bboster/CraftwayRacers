using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class RandomTrapPlacement : NetworkBehaviour
{
    private bool onCooldown = false;

    [SerializeField] private List<Transform> trapPositions;

    [SerializeField] private GameObject trapTest;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject netParent;
    [SerializeField] private GameObject cooldownMarker;

    private void Start()
    {
        cooldownMarker.SetActive(false);
    }

    public void CallFunc()
    {
        SpawnTrapServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnTrapServerRpc()
    {
        if(!onCooldown)
        {
            int choice = Random.Range(0, trapPositions.Count);

            Instantiate(trapTest, parent.transform).transform.localPosition = trapPositions[choice].localPosition;

            GameObject obj = Instantiate(trapTest, netParent.transform);
            obj.GetComponent<NetworkObject>().Spawn(true);
            obj.transform.localPosition = trapPositions[choice].localPosition;


            onCooldown = true;
            cooldownMarker.SetActive(true);
            StartCoroutine(Cooldown());
        }
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
}
