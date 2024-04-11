using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.UI;

public class WinTracker : MonoBehaviour
{
    private bool gameFinished = false;

    [Tooltip("In Seconds")]
    public int gameTime;

    //Waypoints to determine progress around the track.
    public int[] waypoints = new int[4];

    //Laps completed for each player.
    public int[] laps = new int[4];

    public GameObject[] placements = new GameObject[4];
    public int[] winners = new int[4];

    public int playerToWin = -1;

    public GameObject[] players = new GameObject[4];

    [SerializeField] private GameObject winDisplay;
    [SerializeField] private TextMeshProUGUI winTxt;

    private GameObject mainCam;
    private GameObject soundManager;

    [SerializeField] private Sprite player1Sticker;
    [SerializeField] private Sprite player2Sticker;
    [SerializeField] private Sprite player3Sticker;
    [SerializeField] private Sprite player4Sticker;

    [SerializeField] private TextMeshProUGUI player1LapCount;
    [SerializeField] private TextMeshProUGUI player2LapCount;
    [SerializeField] private TextMeshProUGUI player3LapCount;
    [SerializeField] private TextMeshProUGUI player4LapCount;

    [SerializeField] private TextMeshProUGUI timer1;
    [SerializeField] private TextMeshProUGUI timer2;
    [SerializeField] private TextMeshProUGUI timer3;
    [SerializeField] private TextMeshProUGUI timer4;

    private Coroutine timer;

    public GameObject[] wrongWay;
    [SerializeField] private GameObject InputController;

    private void Start()
    {
        mainCam = GameObject.Find("Main Camera");
        soundManager = GameObject.Find("SoundManager");

        WaypointTracking.placementTracking += PlacementTracking_Fire;

        //StartGame();
        DontDestroyOnLoad(gameObject);
    }

    private void PlacementTracking_Fire(int carId, float distToNxtWP, int wpPassed)
    {
        if(InputController.GetComponent<StartCountdown>().Gaming == true)
        {
            int curIndex = Array.IndexOf(placements, players[carId]);
        
            if(curIndex != 0)
            {
                if (waypoints[carId] > waypoints[placements[curIndex - 1].GetComponent<WaypointTracking>().id])
                {
                    GameObject switchObj = placements[curIndex - 1];
                    placements[curIndex] = switchObj;
                    placements[curIndex - 1] = players[carId];
                }
                else if (waypoints[carId] == waypoints[placements[curIndex - 1].GetComponent<WaypointTracking>().id])
                {
                    if (players[carId].GetComponent<WaypointTracking>().distFromNextWP <
                        placements[curIndex - 1].GetComponent<WaypointTracking>().distFromNextWP)
                    {
                        GameObject switchObj = placements[curIndex - 1];
                        placements[curIndex] = switchObj;
                        placements[curIndex - 1] = players[carId];
                    }
                }
            }
        }
    }

    /// <summary>
    /// Starts the game timer.
    /// </summary>
    public void StartGame()
    {
        timer = StartCoroutine(GameTimer());
        //StartCoroutine(WaypointLeadChecker());
    }

    /// <summary>
    /// Adds one to the count of laps a player has completed.
    /// </summary>
    /// <param name="playerNum"> The player that has completed a lap. </param>
    public async void AddLap(int playerNum)
    {
        laps[playerNum]++;
        AudioSource.PlayClipAtPoint(soundManager.GetComponent<SoundManager>().GetSound("Crowd").clip, mainCam.transform.position, 0.3f);

        switch(playerNum)
        {
            case 0:
                player1LapCount.text = laps[playerNum] + " / 2";
                break;
            case 1:
                player2LapCount.text = laps[playerNum] + " / 2";
                break;
            case 2:
                player3LapCount.text = laps[playerNum] + " / 2";
                break;
            case 3:
                player4LapCount.text = laps[playerNum] + " / 2";
                break;
        }

        if(laps[playerNum] == 2)
        {
            /*winDisplay.SetActive(true);
            winTxt.text = "Player " + (playerNum + 1) + " Wins!";*/

            playerToWin = playerNum;

            winners[0] = placements[0].GetComponent<WaypointTracking>().id;
            winners[1] = placements[1].GetComponent<WaypointTracking>().id;
            winners[2] = placements[2].GetComponent<WaypointTracking>().id;
            winners[3] = placements[3].GetComponent<WaypointTracking>().id;
            await LoadScene();
        }
    }

    /// <summary>
    /// Adds one to the count of waypoints a player has passed.
    /// </summary>
    /// <param name="playerNum"> The player that has passed a waypoint. </param>
    public void AddWaypoint(int playerNum)
    {
        waypoints[playerNum]++;
    }

    private IEnumerator GameTimer()
    {
        for(int i = gameTime; i > 0; i--)
        {
            int minutes = i / 60;
            int seconds = i % 60;

            if (seconds == 0)
            {
                timer1.text = minutes + ":00";
                timer2.text = minutes + ":00";
                timer3.text = minutes + ":00";
                timer4.text = minutes + ":00";
            }
            else if (seconds > 9)
            {
                timer1.text = minutes + ":" + seconds;
                timer2.text = minutes + ":" + seconds;
                timer3.text = minutes + ":" + seconds;
                timer4.text = minutes + ":" + seconds;
            }
            else
            {
                timer1.text = minutes + ":0" + seconds;
                timer2.text = minutes + ":0" + seconds;
                timer3.text = minutes + ":0" + seconds;
                timer4.text = minutes + ":0" + seconds;
            }

            if(i <= 15)
            {
                timer1.color = Color.red;
            }

            yield return new WaitForSeconds(1f);
        }

        if(gameFinished == false)
        {
            EndGame();
        }
    }

    private async void EndGame()
    {
        //End the game and determine winner.
        //DetermineWinnerWithLaps();
        winners[0] = placements[0].GetComponent<WaypointTracking>().id;
        winners[1] = placements[1].GetComponent<WaypointTracking>().id;
        if(placements[2] != null)
        {
            winners[2] = placements[2].GetComponent<WaypointTracking>().id;
            if(placements[3] != null)
            {
                winners[3] = placements[3].GetComponent<WaypointTracking>().id;
            }

        }


        await LoadScene();
    }

    /// <summary>
    /// Declares the player with the most completed laps the winner or, if there is a tie, moves on to the next condition.
    /// </summary>
    public async void DetermineWinnerWithLaps()
    {
        bool tiedCondition = false;
        int winner = 0;

        for (int i = 1; i < 4; i++)
        {

            playerToWin = winner;

            if(laps[i] > laps[winner])
            {
                winner = i;
                playerToWin = winner;
                tiedCondition = false;
            }
            else if(laps[i] == laps[winner])
            {
                tiedCondition = true;
            }
        }

        if(tiedCondition)
        {
            DetermineWinnerWithWaypoints();
        }
        else
        {
            /*winDisplay.SetActive(true);
            winTxt.text = "Player " + winner + " Wins!";*/

            winners[0] = placements[0].GetComponent<WaypointTracking>().id;
            winners[1] = placements[1].GetComponent<WaypointTracking>().id;
            winners[2] = placements[2].GetComponent<WaypointTracking>().id;
            winners[3] = placements[3].GetComponent<WaypointTracking>().id;
            await LoadScene();
        }

        //Set win UI.
    }

    private IEnumerator SetWinningSticker(int winner)
    {
        gameFinished = true;
        StopCoroutine(timer);
        GameObject img;

        for(; ; )
        {
            img = GameObject.Find("WinningSticker");

            if(img != null)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        switch(winner)
        {
            case 0:
                img.GetComponent<Image>().sprite = player1Sticker;
                break;
            case 1:
                img.GetComponent<Image>().sprite = player2Sticker;
                break;
            case 2:
                img.GetComponent<Image>().sprite = player3Sticker;
                break;
            case 3:
                img.GetComponent<Image>().sprite = player4Sticker;
                break;
        }
    }

    /// <summary>
    /// Declares the player with the most waypoints completed the winner, or if there is a tie, moves on to the next condition.
    /// </summary>
    private async void DetermineWinnerWithWaypoints()
    {
        bool tiedCondition = false;
        int winner = 0;

        for (int j = 1; j < 4; j++)
        {
            if(waypoints[j] > waypoints[winner])
            {
                winner = j;
                playerToWin = winner;
                tiedCondition = false;
            }
            else if(waypoints[j] == waypoints[winner])
            {
                tiedCondition = true;
            }
        }

        if(tiedCondition)
        {
            DetermineWinnerWithDistance();
        }
        else
        {
            /*winDisplay.SetActive(true);
            winTxt.text = "Player " + winner + " Wins!";*/

            await LoadScene();
        }

        //Set winner UI.
    }

    /// <summary>
    /// Declares the winner based on their distance to the next waypoint.
    /// </summary>
    private async void DetermineWinnerWithDistance()
    {
        bool tiedCondition = false;
        int winner = 0;
        
        for(int k = 1; k < 4; k++)
        {
            if (players[k].GetComponent<WaypointTracking>().distFromNextWP > players[winner].GetComponent<WaypointTracking>().distFromNextWP)
            {
                winner = k;
                playerToWin = winner;
                tiedCondition = false;
            }
            else if(players[k].GetComponent<WaypointTracking>().distFromNextWP == players[winner].GetComponent<WaypointTracking>().distFromNextWP)
            {
                tiedCondition = true;
            }
        }

        if(tiedCondition)
        {
            //Tied UI display.
        }
        else
        {
            /*winDisplay.SetActive(true);
            winTxt.text = "Player " + winner + " Wins!";*/

            await LoadScene();
        }
    }

    private Task LoadScene()
    {
        SceneManager.LoadScene("ResultsScene");

        return Task.CompletedTask;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            GameObject.Find("ResultsController").GetComponent<ResultScreenManager>().DisplayWinners(gameObject.GetComponent<WinTracker>());
        }
    }

    //CHECK WHO IS IN THE LEAD EVERY FRAME HERE AND ADJUST UI ELEMENTS.

    /*private IEnumerator WaypointLeadChecker()
    {
        for (; ; )
        {
            int winner = 0;
            bool tiedCondition = false;

            List<int> tiedIndexes = new List<int>();
            List<int> indexesToSkip = new List<int>();

            foreach(int i in placements)
            {
                winner = 0;

                for(int index = 0; index < 4; )
                {
                    if(index != winner && indexesToSkip.IndexOf(index) == -1 && indexesToSkip.IndexOf(winner) == -1)
                    {
                        if(waypoints[index] > waypoints[winner])
                        {
                            winner = index;
                            tiedCondition = false;
                            index++;
                        }
                        else if(waypoints[index] == waypoints[winner])
                        {
                            tiedCondition = true;
                            tiedIndexes.Add(winner);
                            tiedIndexes.Add(index);
                            index++;
                        }
                        else
                        {
                            index++;
                        }
                    }
                    else if(indexesToSkip.IndexOf(winner) != -1)
                    {
                        winner++;
                    }
                    else
                    {
                        index++;
                    }
                }

                if(tiedCondition)
                {
                    //REMOVE BYPASS ONCE SYSTEM IS COMPLETE.
                    goto Bypass;
                    TieResolver(tiedIndexes, indexesToSkip, i);
                    continue;

                Bypass:
                    continue;
                }
                else
                {
                    placements[Array.IndexOf(placements, i)] = winner;
                    indexesToSkip.Add(winner);
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void TieResolver(List<int> tiedIndexes, List<int> indexesToSkip, int currentPlacement)
    {
        
    }*/
}
