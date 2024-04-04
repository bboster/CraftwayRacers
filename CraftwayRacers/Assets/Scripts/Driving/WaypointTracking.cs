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

    private Coroutine wrongWayTimer;

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

            if(oldDist < distFromNextWP && GetComponent<Rigidbody>().velocity.x + GetComponent<Rigidbody>().velocity.z > 2)
            {
                if(wrongWayTimer == null)
                {
                    //Show wrong direction UI.
                    wrongWayTimer = StartCoroutine(WrongWayTimer());
                    Debug.Log("WRONG WAY");
                }
            }
            else
            {
                if(wrongWayTimer != null)
                {
                    gc.GetComponent<WinTracker>().wrongWay[id].SetActive(false);
                    StopCoroutine(wrongWayTimer);
                    wrongWayTimer = null;
                }
            }

            yield return new WaitForSeconds(1.5f);
        }

    }

    private IEnumerator WrongWayTimer()
    {
        for(int i = 3; i > 0; i--)
        {
            yield return new WaitForSeconds(1f);
        }

        gc.GetComponent<WinTracker>().wrongWay[id].SetActive(true);
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
