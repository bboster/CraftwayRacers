using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using TMPro;

public class RelayServerController : MonoBehaviour
{
    [SerializeField] private GameObject display;
    [SerializeField] private TextMeshProUGUI codeDisplay;

    [SerializeField] private TMP_InputField joinCodeInput;
    [SerializeField] private GameObject input;

    [SerializeField] private GameObject spawnManager;
    [SerializeField] private GameObject testCube;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => { Debug.Log("Signed In"); };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);

            StartCoroutine(DisplayJoinCode(joinCode));

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

            GameObject s = Instantiate(spawnManager);
            s.GetComponent<NetworkObject>().Spawn(true);

            input.SetActive(false);
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    private IEnumerator DisplayJoinCode(string code)
    {
        codeDisplay.text = code;

        yield return new WaitForSeconds(5f);

        display.SetActive(false);
    }

    public async void JoinRelay()
    {
        try
        {
            testCube.SetActive(true);

            string joinCode = joinCodeInput.text;

            await RelayService.Instance.JoinAllocationAsync(joinCode);

            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "udp");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();

            testCube.SetActive(false);

            input.SetActive(false);
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
}