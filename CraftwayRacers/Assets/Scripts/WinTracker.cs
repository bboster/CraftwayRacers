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
        for(int i = 0; i < 120; i++)
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
        List<int> tiedPlayers = new List<int>();

        for(int i = 1; i < 4; i++)
        {
            int winner = 0;
            playerToWin = winner;

            if(laps[i] > laps[winner])
            {
                winner = i;
                playerToWin = winner;
                tiedCondition = false;
                tiedPlayers.Clear();
            }
            else if(laps[i] == laps[winner])
            {
                tiedCondition = true;
                tiedPlayers.Add(i);
                tiedPlayers.Add(winner);
            }
        }

        if(tiedCondition)
        {
            DetermineWinnerWithWaypoints(tiedPlayers);
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
    /// <param name="tied"></param>
    private void DetermineWinnerWithWaypoints(List<int> tied)
    {
        bool tiedCondition = false;
        List<int> tiedPlayers = new List<int>();

        for(int i = 1; i < tied.Count; i++)
        {
            int winner = 0;

            if(waypoints[i] > waypoints[winner])
            {
                winner = i;
                playerToWin = winner;
                tiedCondition = false;
                tiedPlayers.Clear();
            }
            else if(waypoints[i] == waypoints[winner])
            {
                tiedCondition = true;
                tiedPlayers.Add(i);
                tiedPlayers.Add(winner);
            }
        }

        if(tiedCondition)
        {
            DetermineWinnerWithDistance(tiedPlayers);
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
    /// <param name="tied"></param>
    private void DetermineWinnerWithDistance(List<int> tied)
    {
        //Get distance to the next waypoint for each player.
    }
}
