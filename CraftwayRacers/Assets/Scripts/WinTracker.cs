using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTracker : MonoBehaviour
{
    //Waypoints to determine progress around the track.
    public int[] waypoints = new int[4];

    //Laps completed for each player.
    public int[] laps = new int[4];

    private int playerToWin = -1;

    public GameObject[] players = new GameObject[4];

    private void Start()
    {
        StartGame();
    }

    /// <summary>
    /// Starts the game timer.
    /// </summary>
    public void StartGame()
    {
        StartCoroutine(GameTimer());
    }

    /// <summary>
    /// Adds one to the count of laps a player has completed.
    /// </summary>
    /// <param name="playerNum"> The player that has completed a lap. </param>
    public void AddLap(int playerNum)
    {
        laps[playerNum]++;
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
        for(int i = 0; i < 30; i++)
        {
            yield return new WaitForSeconds(1f);
        }

        //End the game and determine winner.
        DetermineWinnerWithLaps();
    }

    /// <summary>
    /// Declares the player with the most completed laps the winner or, if there is a tie, moves on to the next condition.
    /// </summary>
    public void DetermineWinnerWithLaps()
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
            print(playerToWin);
        }

        //Set win UI.
    }

    /// <summary>
    /// Declares the player with the most waypoints completed the winner, or if there is a tie, moves on to the next condition.
    /// </summary>
    private void DetermineWinnerWithWaypoints()
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
            print(playerToWin);
        }

        //Set winner UI.
    }

    /// <summary>
    /// Declares the winner based on their distance to the next waypoint.
    /// </summary>
    private void DetermineWinnerWithDistance()
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
            print(playerToWin);
        }
    }
}
