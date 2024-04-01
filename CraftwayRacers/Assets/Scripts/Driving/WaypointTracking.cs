using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaypointTracking : MonoBehaviour
{
    [SerializeField] private int nextWaypoint = 0;
    [SerializeField] private int id;

    public float distFromNextWP;
    private float oldDist;

    private GameObject gc;
    private GameObject[] waypoints;

    [SerializeField] private GameObject wrongWay;

    void Start()
    {
        waypoints = GameObject.Find("GameController").GetComponent<GameController>().waypointTriggers;
        gc = GameObject.Find("GameController");

        id = GameObject.FindGameObjectsWithTag("Player").Length - 1;

        gc.GetComponent<WinTracker>().players[id] = gameObject;

        print(id);

        StartCoroutine(DirectionTracker());
    }

    private IEnumerator DirectionTracker()
    {
        for(; ; )
        {
            oldDist = distFromNextWP;
            distFromNextWP = Vector3.Distance(transform.position, waypoints[nextWaypoint].transform.position);

            if(oldDist < distFromNextWP)
            {
                //Show wrong direction UI.

                Debug.Log("WRONG WAY");
                
            }
            else
            {
                
            }

            yield return new WaitForSeconds(5f);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Waypoint"))
        {
            if(nextWaypoint < waypoints.Length - 1 && nextWaypoint == other.gameObject.GetComponent<WaypointID>().waypointID)
            {
                nextWaypoint++;
                gc.GetComponent<WinTracker>().waypoints[id]++;
            }

            //Increase counter for waypoints.
        }
        else if(other.gameObject.CompareTag("LapTrigger") && nextWaypoint == other.gameObject.GetComponent<WaypointID>().waypointID)
        {
            nextWaypoint = 0;
            gc.GetComponent<WinTracker>().waypoints[id]++;
            gc.GetComponent<WinTracker>().AddLap(id);
        }
    }
}
