using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StartCountdown : MonoBehaviour
{
    public GameObject Join;
    public GameObject OnePlayer;
    private bool oneHasRun;
    public GameObject TwoPlayer;
    private bool twoHasRun;
    public GameObject ThreePlayer;
    private bool threeHasRun;
    public GameObject Ready;
    public GameObject One;
    public GameObject Two;
    public GameObject Three;
    public GameObject GO;
    public GameObject OneStopper;
    public GameObject TwoStopper;
    public GameObject ThreeStopper;
    public GameObject FourStopper;

    private GameObject mainCam;
    private GameObject soundManager;
    private GameObject musicPlayer;

    public static Action StartRace;

    public PlayerInputManager PlayerCounting;
    private bool countDownHasRun = false;
    public bool Gaming = false;
    public int Players;

    [SerializeField]
    private List<LayerMask> playerLayers;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.Find("Main Camera");
        soundManager = GameObject.Find("SoundManager");
        musicPlayer = GameObject.Find("MusicPlayer");

        Join.SetActive(true);
        /*if (PlayerCounting.playerCount == 4)
        {
            StartCoroutine(ReadySetGo());
        }*/
    }

    IEnumerator ReadySetGo()
    {
        ThreePlayer.SetActive(false);
        Ready.SetActive(true);

        yield return new WaitForSeconds(2f);
        Ready.SetActive(false);
        Three.SetActive(true);
        AudioSource.PlayClipAtPoint(soundManager.GetComponent<SoundManager>().GetSound("Countdown").clip, mainCam.transform.position);

        yield return new WaitForSeconds(1);
        Three.SetActive(false);
        Two.SetActive(true);
        AudioSource.PlayClipAtPoint(soundManager.GetComponent<SoundManager>().GetSound("Countdown").clip, mainCam.transform.position);

        yield return new WaitForSeconds(1);
        Two.SetActive(false);
        One.SetActive(true);
        AudioSource.PlayClipAtPoint(soundManager.GetComponent<SoundManager>().GetSound("Countdown").clip, mainCam.transform.position);

        yield return new WaitForSeconds(1f);
        One.SetActive(false);
        OneStopper.SetActive(false);
        TwoStopper.SetActive(false);
        ThreeStopper.SetActive(false);
        FourStopper.SetActive(false);
        Gaming = true;
        StartRace?.Invoke();
        GO.SetActive(true);
        AudioSource.PlayClipAtPoint(soundManager.GetComponent<SoundManager>().GetSound("Go").clip, mainCam.transform.position);
        musicPlayer.GetComponent<AudioSource>().enabled = true;


        GameObject.Find("GameController").GetComponent<WinTracker>().StartGame();
        
        yield return new WaitForSeconds(2f);
        GO.SetActive(false);

    }
    
    // Update is called once per frame
    void Update()
    {
        P1();
        P2();
        P3();
        CountDown();

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ReadySetGo());
        }*/
    }

    void P1()
    {
        if (PlayerCounting.playerCount == 1 && oneHasRun == false)
        {
            Join.SetActive(false);
            OnePlayer.SetActive(true);
            oneHasRun = true;
        }
    }

    void P2()
    {
        if (PlayerCounting.playerCount == 2 && twoHasRun == false)
        {
            OnePlayer.SetActive(false);
            TwoPlayer.SetActive(true);
            twoHasRun = true;
        }
    }

    void P3()
    {
        if (PlayerCounting.playerCount == 3 && threeHasRun == false)
        {
            TwoPlayer.SetActive(false);
            ThreePlayer.SetActive(true);
            threeHasRun = true;
        }
    }
    void CountDown()
    {
        if (PlayerCounting.playerCount == Players && countDownHasRun == false)
        {
            StartCoroutine(ReadySetGo()); //Call this coroutine with a press of the button 
            countDownHasRun = true;
        }
    }


}
