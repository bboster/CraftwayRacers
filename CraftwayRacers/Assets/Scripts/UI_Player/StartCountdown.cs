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
    public GameObject FourPlayer;
    private bool fourHasRun;
    public GameObject Ready;
    public GameObject One;
    public GameObject Two;
    public GameObject Three;
    public GameObject GO;
    public GameObject OppaStoppas; //Game objects that stop the player from moving before the game starts

    [SerializeField] private GameObject p1JoinImg;
    [SerializeField] private GameObject p2JoinImg;
    [SerializeField] private GameObject p3JoinImg;
    [SerializeField] private GameObject p4JoinImg;

    [SerializeField] private Animator a;

    private GameObject mainCam;
    private GameObject soundManager;
    private GameObject musicPlayer;

    public static Action StartRace;

    public PlayerInputManager PlayerCounting;
    public bool countDownHasRun = false; // changed this to public
    public bool Gaming = false;
    public int Players;

    [SerializeField]
    private List<LayerMask> playerLayers;

    // Start is called before the first frame update
    void Start()
    {
        a = GameObject.Find("JoinScreen").GetComponent<Animator>();

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
        a.enabled = true;

        OnePlayer.SetActive(false);
        TwoPlayer.SetActive(false);
        ThreePlayer.SetActive(false);
        FourPlayer.SetActive(false);
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
        OppaStoppas.SetActive(false);
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
        P4();
 

        if (Input.GetKeyDown(KeyCode.Space))
        {
           CountDown();
        }
    }

    void P1()
    {
        if (PlayerCounting.playerCount == 1 && oneHasRun == false)
        {
            Join.SetActive(false);
            OnePlayer.SetActive(true);
            p1JoinImg.SetActive(true);
            oneHasRun = true;
            AudioSource.PlayClipAtPoint(soundManager.GetComponent<SoundManager>().GetSound("Join").clip, mainCam.transform.position);

        }
    }

    void P2()
    {
        if (PlayerCounting.playerCount == 2 && twoHasRun == false)
        {
            OnePlayer.SetActive(false);
            TwoPlayer.SetActive(true);
            p2JoinImg.SetActive(true);
            twoHasRun = true;
            AudioSource.PlayClipAtPoint(soundManager.GetComponent<SoundManager>().GetSound("Join").clip, mainCam.transform.position);
        }
    }

    void P3()
    {
        if (PlayerCounting.playerCount == 3 && threeHasRun == false)
        {
            TwoPlayer.SetActive(false);
            ThreePlayer.SetActive(true);
            p3JoinImg.SetActive(true);
            threeHasRun = true;
        }
    }

    void P4()
    {
        if (PlayerCounting.playerCount == 4 && fourHasRun == false)
        {
            ThreePlayer.SetActive(false);
            FourPlayer.SetActive(true);
            p4JoinImg.SetActive(true);
            fourHasRun = true;
            AudioSource.PlayClipAtPoint(soundManager.GetComponent<SoundManager>().GetSound("Join").clip, mainCam.transform.position);
        }
    }
    void CountDown()
    {
            StartCoroutine(ReadySetGo()); //Call this coroutine with a press of the button 
            countDownHasRun = true;
    }
}
