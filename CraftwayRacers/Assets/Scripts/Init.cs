using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Init : MonoBehaviour
{
    [SerializeField] private GameObject pcCam;
    [SerializeField] private GameObject ipadCam;

    [SerializeField] private GameObject iPadUI;
    [SerializeField] private GameObject iPadMap;

    [SerializeField] private GameObject relay;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
           ipadCam.SetActive(true);
           ipadCam.GetComponent<CinemachineVirtualCamera>().Priority = 100;
        }
        else
        {

        }

        Trap1 t = new Trap1(new Vector3(0, 1, 2));

        print(t.ID);
        print(t.Position);

        relay.SetActive(true);
    }
}
