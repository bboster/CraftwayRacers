using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningSystem : MonoBehaviour
{
    // The list of spawn points.
    public List<Vector3> spawnPoints;
    public List<GameObject> largeHazards;
    //public List<GameObject> gatchaBalls;
    //public List<Vector3> gatchaSpawns;

    // Spawn points for disruptions that can be changed in the inspector.
    [SerializeField] private Vector3 spawnPoint1;
    [SerializeField] private Vector3 spawnPoint2;
    [SerializeField] private Vector3 spawnPoint3;
    [SerializeField] private Vector3 spawnPoint4;
    [SerializeField] private Vector3 spawnPoint5;
    [SerializeField] private Vector3 spawnPoint6;
    [SerializeField] private Vector3 spawnPoint7;
    [SerializeField] private Vector3 spawnPoint8;
    [SerializeField] private Vector3 spawnPoint9;
    [SerializeField] private Vector3 spawnPoint10;
    [SerializeField] private Vector3 spawnPoint11;
    [SerializeField] private Vector3 spawnPoint12;
    [SerializeField] private Vector3 spawnPoint13;
    [SerializeField] private Vector3 spawnPoint14;

    // Objects that are spawned
    //[SerializeField] private GameObject TestCube; Caleb was here
    [SerializeField] private GameObject BoosterGatcha;
    [SerializeField] private GameObject CottonBallShieldGatcha;
    [SerializeField] private GameObject Jacks;
    [SerializeField] private GameObject Thumbtack;
    //[SerializeField] private GameObject RubberBand; Caleb was here

    // Amount of wait time
    [SerializeField] private int WaitTime;

    public bool IsWaiting = true;
    public bool IsWaitingGatcha = true;

    public ThumbtackBehavior TB;

    private int count;

    public StartCountdown SC;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints.Add(spawnPoint1);
        spawnPoints.Add(spawnPoint2);
        spawnPoints.Add(spawnPoint3);
        spawnPoints.Add(spawnPoint4);
        spawnPoints.Add(spawnPoint5);
        spawnPoints.Add(spawnPoint6);
        spawnPoints.Add(spawnPoint7);
        spawnPoints.Add(spawnPoint8);
        spawnPoints.Add(spawnPoint9);
        spawnPoints.Add(spawnPoint10);
        spawnPoints.Add(spawnPoint11);
        spawnPoints.Add(spawnPoint12);
        spawnPoints.Add(spawnPoint13);
        spawnPoints.Add(spawnPoint14);
        //gatchaSpawns.Add(spawnPoint8);
        //gatchaSpawns.Add(spawnPoint9);
        //gatchaSpawns.Add(spawnPoint10);
        //gatchaSpawns.Add(spawnPoint11);
        //gatchaSpawns.Add(spawnPoint12);
        //gatchaSpawns.Add(spawnPoint13);
        //gatchaSpawns.Add(spawnPoint14);



        largeHazards.Add(Thumbtack);
        largeHazards.Add(Jacks);
        largeHazards.Add(BoosterGatcha); //Caleb changed "Paintbrush" to "Booster"
        largeHazards.Add(CottonBallShieldGatcha);

        //gatchaBalls.Add(PaintBrushGatcha);
        //gatchaBalls.Add(CottonBallShieldGatcha);
    }

    // Update is called once per frame
    void Update()
    {
         if (IsWaiting == true && spawnPoints.Count > 0 && SC.countDownHasRun == true)
         {
             StartCoroutine(LargeHazardsTimer());
         }
    }

    public IEnumerator LargeHazardsTimer()
    {
        IsWaiting = false;
        GameObject largeHazard;
        largeHazard = ChooseLargeHazard();
        yield return new WaitForSeconds(WaitTime);
        Instantiate(largeHazard, PlaceToSpawn(), Quaternion.identity);
        IsWaiting = true;
    }

    //IEnumerator GatchaBallTimer()
    //{
    //    IsWaitingGatcha = false;
    //    GameObject gatchaBall;
    //    gatchaBall = ChooseGatchaBall();
    //    yield return new WaitForSeconds(WaitTime);
    //    Instantiate(gatchaBall, PlaceToSpawnGatcha(), Quaternion.identity);
    //    IsWaitingGatcha = true;
    //}

    private Vector3 PlaceToSpawn()
    {
        //int number = Random.Range(0, spawnPoints.Count - 1);
        //spawnPoints.Remove(spawnPoints[number]);
        //return spawnPoints[number];
        //if (spawnPoints.Count == 0)
        //{
        //    Debug.LogError("No more spawn points available!");
        //}

        int index = Random.Range(0, spawnPoints.Count);
        Vector3 spawnPosition = spawnPoints[index];
        spawnPoints.RemoveAt(index);  // Remove the selected spawn point from the list
        return spawnPosition;
    }

    //private Vector3 PlaceToSpawnGatcha()
    //{
    //    int number = Random.Range(0, gatchaSpawns.Count - 1);
    //    gatchaSpawns.Remove(gatchaSpawns[number]);  
    //    return gatchaSpawns[number];
    //}

    private GameObject ChooseLargeHazard()
    {
        int number = Random.Range(0, largeHazards.Count);
        return largeHazards[number];
    }

    //private GameObject ChooseGatchaBall()
    //{
    //    int number = Random.Range(0, gatchaBalls.Count);
    //    return gatchaBalls[number];
    //}


}
