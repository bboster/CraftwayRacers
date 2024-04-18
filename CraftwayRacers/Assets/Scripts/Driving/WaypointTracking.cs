using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaypointTracking : MonoBehaviour
{
    [SerializeField] private int nextWaypoint = 0;
    public int id;
    [SerializeField] private int wrongWaypointNum = 16;
    [SerializeField] private int waypointsPassed;

    public float distFromNextWP;
    [SerializeField] private float distFromWrongWaypoint;
    private float oldDist;

    private GameObject gc;
    private GameObject[] waypoints;

    private Coroutine wrongWayTimer;
    private Coroutine wrongWayStopper;

    public static Action<int, float, int> placementTracking;

    void Start()
    {
        waypoints = GameObject.Find("GameController").GetComponent<GameController>().waypointTriggers;
        gc = GameObject.Find("GameController");

        id = GameObject.FindGameObjectsWithTag("Player").Length - 1;
        gc.GetComponent<WinTracker>().placements[id] = gameObject;
        Debug.Log(gc.GetComponent<WinTracker>().placements[id]);

        gc.GetComponent<WinTracker>().players[id] = gameObject;

        print(id);

        StartCoroutine(DirectionTracker());
    }

    private void Update()
    {
        placementTracking?.Invoke(id, distFromNextWP, waypointsPassed);
    }

    private IEnumerator DirectionTracker()
    {
        for(; ; )
        {
            oldDist = distFromWrongWaypoint;
            distFromWrongWaypoint = Vector3.Distance(transform.position, waypoints[wrongWaypointNum].transform.position);
            distFromNextWP = Vector3.Distance(transform.position, waypoints[nextWaypoint].transform.position);

            if(oldDist > distFromWrongWaypoint && Mathf.Abs(GetComponent<Rigidbody>().velocity.x + GetComponent<Rigidbody>().velocity.z) > 2)
            {
                if(wrongWayTimer == null)
                {
                    //Show wrong direction UI.
                    wrongWayTimer = StartCoroutine(WrongWayTimer());

                    if(wrongWayStopper != null)
                    {
                        StopCoroutine(wrongWayStopper);
                        wrongWayStopper = null;
                    }

                    Debug.Log("WRONG WAY");
                }
            }
            else
            {
                if(wrongWayTimer != null)
                {
                    StopCoroutine(wrongWayTimer);

                    if(wrongWayStopper == null)
                    {
                        wrongWayStopper = StartCoroutine(StopWrongWayTimer());
                    }

                    //StopCoroutine(wrongWayTimer);
                    wrongWayTimer = null;
                }
            }

            yield return new WaitForEndOfFrame();
        }

    }

    private IEnumerator WrongWayTimer()
    {
        for(float i = 1.5f; i > 0; i -= 0.5f)
        {
            yield return new WaitForSeconds(0.5f);
        }

        gc.GetComponent<WinTracker>().wrongWay[id].SetActive(true);
    }

    private IEnumerator StopWrongWayTimer()
    {
        for(float i = 1.5f; i > 0; i -= 0.5f)
        {
            yield return new WaitForSeconds(0.5f);
        }

        gc.GetComponent<WinTracker>().wrongWay[id].SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Waypoint"))
        {
            if(nextWaypoint < waypoints.Length - 1 && nextWaypoint == other.gameObject.GetComponent<WaypointID>().waypointID)
            {
                nextWaypoint++;
                gc.GetComponent<WinTracker>().waypoints[id]++;

                wrongWaypointNum = other.gameObject.GetComponent<WaypointID>().waypointID;
            }
            else if(other.gameObject.GetComponent<WaypointID>().waypointID == wrongWaypointNum)
            {
                if(wrongWaypointNum > 0)
                {
                    wrongWaypointNum = other.gameObject.GetComponent<WaypointID>().waypointID - 1;
                    //gc.GetComponent<WinTracker>().waypoints[id]--;
                }
                else if (other.gameObject.GetComponent<WaypointID>().waypointID == 0)
                {
                    wrongWaypointNum = 17;
                    //gc.GetComponent<WinTracker>().waypoints[id]--;
                }
            }
            else if(other.gameObject.GetComponent<WaypointID>().waypointID == wrongWaypointNum + 1)
            {
                wrongWaypointNum = other.gameObject.GetComponent<WaypointID>().waypointID;
            }

            //Increase counter for waypoints.
        }
        else if(other.gameObject.CompareTag("LapTrigger") && nextWaypoint == other.gameObject.GetComponent<WaypointID>().waypointID)
        {
            nextWaypoint = 0;
            gc.GetComponent<WinTracker>().waypoints[id]++;
            gc.GetComponent<WinTracker>().AddLap(id);
        }
        else if(other.gameObject.CompareTag("LapTrigger") && wrongWaypointNum == 17)
        {
            wrongWaypointNum = other.gameObject.GetComponent<WaypointID>().waypointID - 1;
            //gc.GetComponent<WinTracker>().waypoints[id]--;
        }
    }
}
