using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class Init : MonoBehaviour
{
    private int minutes = 2;
    private int seconds = 00;

    //[SerializeField] private GameObject pcCam;
    [SerializeField] private GameObject ipadCam;

    [SerializeField] private GameObject iPadUI;
    [SerializeField] private GameObject iPadMap;
    [SerializeField] private GameObject codeTxt;

    [SerializeField] private GameObject restartBtn;
    [SerializeField] private GameObject spawnBtn;
    [SerializeField] private GameObject carSpawnBtn;

    [SerializeField] private GameObject relay;

    [SerializeField] private TextMeshProUGUI timerTxt;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            ipadCam.SetActive(true);
            ipadCam.GetComponent<CinemachineVirtualCamera>().Priority = 100;
            iPadUI.SetActive(true);
            iPadMap.SetActive(true);
            codeTxt.SetActive(false);
        }
        else
        {
            StartCoroutine(Timer());
        }

        //Trap1 t = new Trap1(new Vector3(0, 1, 2));

        //print(t.ID);
        //print(t.Position);

        relay.SetActive(true);
    }

    private IEnumerator Timer()
    {
        timerTxt.text = minutes + ":" + seconds;

        yield return new WaitForSeconds(1f);
    }
}
