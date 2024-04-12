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

    // Controls existence of thumbtacks
    [SerializeField] private int thumbtackAmount = 0;
    [SerializeField] private int maxAmountThumbtack;
    [SerializeField] private int jacksAmount = 0;
    [SerializeField] private int maxAmountJacks;
    public int gatchaAmount = 1;
    public List<GameObject> GatchaList;

    public List<Vector3> respawns = new List<Vector3>();
    private int number;

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
   
        largeHazards.Add(Thumbtack);
        largeHazards.Add(Jacks);
        largeHazards.Add(BoosterGatcha); //Caleb changed "Paintbrush" to "Booster"
        largeHazards.Add(CottonBallShieldGatcha);

        GatchaList.Add(BoosterGatcha);
        GatchaList.Add(CottonBallShieldGatcha);
    }

    // Update is called once per frame
    void Update()
    {

        if (IsWaiting == true && spawnPoints.Count > 0 && SC.countDownHasRun == true && gatchaAmount >= 0)
        {
            StartCoroutine(LargeHazardsTimer());
            if(gatchaAmount == 0)
            {
                gatchaAmount--;
            }
        }
        if (IsWaiting == true && respawns.Count > 0 && SC.countDownHasRun == true && gatchaAmount < 0)
        {
            StartCoroutine(GatchaTimer());
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

    public IEnumerator GatchaTimer()
    {
        IsWaiting = false;
        GameObject gatcha;
        gatcha = ChooseGatcha();
        yield return new WaitForSeconds(WaitTime);
        Instantiate(gatcha, PlaceToSpawnGatcha(), Quaternion.identity);
        IsWaiting = true;
    }

    private Vector3 PlaceToSpawn()
    {
        int index = Random.Range(0, spawnPoints.Count);
        Vector3 spawnPosition = spawnPoints[index];
        spawnPoints.RemoveAt(index);  // Remove the selected spawn point from the list
        return spawnPosition;
    }

    private GameObject ChooseGatcha()
    {
        number = Random.Range(0, GatchaList.Count);
        return GatchaList[number];
    }

    private Vector3 PlaceToSpawnGatcha()
    {
        int index = Random.Range(0, respawns.Count);
        Vector3 respawnPosition = respawns[index];
        respawns.RemoveAt(index);
        return respawnPosition;
    }
    private GameObject ChooseLargeHazard()
    {
        number = Random.Range(0, largeHazards.Count);
        if (largeHazards[number] == Thumbtack)
        {
            if(thumbtackAmount >= maxAmountThumbtack)
            {
                largeHazards.RemoveAt(number);
            }
            thumbtackAmount++;
        }
        else if (largeHazards[number] == Jacks)
        {
            if(jacksAmount >= maxAmountJacks)
            {
                largeHazards.RemoveAt(number);
            }
            jacksAmount++;
        }
        else if (largeHazards[number] == BoosterGatcha || largeHazards[number] == CottonBallShieldGatcha)
        {
            gatchaAmount++;
            respawns.Add(largeHazards[number].transform.position);
        }
        return largeHazards[number];
    }


}
