using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Init : MonoBehaviour
{
    [SerializeField] private GameObject pcCam;
    [SerializeField] private GameObject ipadCam;

    [SerializeField] private GameObject relay;

    private void Awake()
    {
        if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            ipadCam.SetActive(true);
            ipadCam.GetComponent<CinemachineVirtualCamera>().Priority = 100;
        }
        else
        {
            
        }

        relay.SetActive(true);
    }
}
