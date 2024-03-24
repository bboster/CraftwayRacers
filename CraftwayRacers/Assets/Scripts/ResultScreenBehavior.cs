using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultScreenBehavior : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winTxt;

    private void Start()
    {
        winTxt.text = "Player " + GameObject.Find("GameController").GetComponent<WinTracker>().playerToWin + " wins!";
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("OfficialPlaytestScene");
    }
}
