using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreenManager : MonoBehaviour
{
    [SerializeField] private Sprite player1Sticker;
    [SerializeField] private Sprite player2Sticker;
    [SerializeField] private Sprite player3Sticker;
    [SerializeField] private Sprite player4Sticker;

    [SerializeField] private GameObject win1;
    [SerializeField] private GameObject win2;
    [SerializeField] private GameObject win3;
    [SerializeField] private GameObject win4;

    [SerializeField] private Animator objToFade;

    private void Start()
    {
        StartCoroutine(NextSceneTimer());
    }

    public void DisplayWinners(WinTracker wt)
    {
        GameObject sticker1 = GameObject.Find("Sticker1");
        GameObject sticker2 = GameObject.Find("Sticker2");
        GameObject sticker3 = GameObject.Find("Sticker3");
        GameObject sticker4 = GameObject.Find("Sticker4");

        switch (wt.winners[0])
        {
            case 0:
                win1.SetActive(true);
                sticker1.GetComponent<Image>().sprite = player1Sticker;
                break;
            case 1:
                win2.SetActive(true);
                sticker1.GetComponent<Image>().sprite = player2Sticker;
                break;
            case 2:
                win3.SetActive(true);
                sticker1.GetComponent<Image>().sprite = player3Sticker;
                break;
            case 3:
                win4.SetActive(true);
                sticker1.GetComponent<Image>().sprite = player4Sticker;
                break;
        }

        switch (wt.winners[1])
        {
            case 0:
                sticker2.GetComponent<Image>().sprite = player1Sticker;
                break;
            case 1:
                sticker2.GetComponent<Image>().sprite = player2Sticker;
                break;
            case 2:
                sticker2.GetComponent<Image>().sprite = player3Sticker;
                break;
            case 3:
                sticker2.GetComponent<Image>().sprite = player4Sticker;
                break;
        }

        switch (wt.winners[2])
        {
            case 0:
                sticker3.GetComponent<Image>().sprite = player1Sticker;
                break;
            case 1:
                sticker3.GetComponent<Image>().sprite = player2Sticker;
                break;
            case 2:
                sticker3.GetComponent<Image>().sprite = player3Sticker;
                break;
            case 3:
                sticker3.GetComponent<Image>().sprite = player4Sticker;
                break;
        }

        switch (wt.winners[3])
        {
            case 0:
                sticker4.GetComponent<Image>().sprite = player1Sticker;
                break;
            case 1:
                sticker4.GetComponent<Image>().sprite = player2Sticker;
                break;
            case 2:
                sticker4.GetComponent<Image>().sprite = player3Sticker;
                break;
            case 3:
                sticker4.GetComponent<Image>().sprite = player4Sticker;
                break;
        }
    }

    private IEnumerator NextSceneTimer()
    {
        yield return new WaitForSeconds(7f);

        objToFade.enabled = true;

        yield return new WaitForSeconds(10f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
