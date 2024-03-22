using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StartCountdown : MonoBehaviour
{
    public GameObject Ready;
    public GameObject One;
    public GameObject Two;
    public GameObject Three;
    public GameObject GO;

    public PlayerInputManager PlayerCounting;
    private bool hasRun = false;
    public bool Gaming = false;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerCounting.playerCount == 4)
        {
            StartCoroutine(ReadySetGo());
        }
    }

    IEnumerator ReadySetGo()
    {
        Ready.SetActive(true);

        yield return new WaitForSeconds(2f);
        Ready.SetActive(false);
        Three.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        Three.SetActive(false);
        Two.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        Two.SetActive(false);
        One.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        One.SetActive(false);
        Gaming = true;
        GO.SetActive(true);
        
        yield return new WaitForSeconds(2f);
        GO.SetActive(false);


    }
    
    // Update is called once per frame
    void Update()
    {
        StartGame();

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ReadySetGo());
        }*/
    }

    void StartGame()
    {
        if (PlayerCounting.playerCount == 4 && hasRun == false)
        {
            StartCoroutine(ReadySetGo());
            hasRun = true;
        }
    }
}
