using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTracker : MonoBehaviour
{
    //Waypoints to determine progress around the track.
    [SerializeField] private int[] waypoints;

    //Amount of waypoints passed by each player.
    [SerializeField] private int[] pointsPassed;

    //Laps completed for each player.
    [SerializeField] private int[] laps;

    public void AddLap(int playerNum)
    {
        laps[playerNum]++;
    }

    public void AddWaypoint(int playerNum)
    {
        waypoints[playerNum]++;
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

            if(laps[i] > laps[winner])
            {
                winner = i;
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
