using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningSystem : MonoBehaviour
{
    // The list of spawn points.
    public List<Vector3> spawnPoints;
    public List<GameObject> largeHazards;

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
    [SerializeField] private GameObject TestCube;
    [SerializeField] private GameObject PaintBrushGatcha;
    [SerializeField] private GameObject CottonBallShieldGatcha;
    [SerializeField] private GameObject Jacks;
    [SerializeField] private GameObject Thumbtack;
    [SerializeField] private GameObject RubberBand;

    // Amount of wait time
    [SerializeField] private int WaitTime;

    private bool IsWaiting = true;

    public ThumbtackBehavior TB;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (TB.isOnGround == true)
        {
            StartCoroutine(Wait());
            
        }
        if(TB.isOnGround == false)
        {
            StartCoroutine(Wait2());
        }
    }

    //IEnumerator LargeHazardsTimer()
    //{
    //    GameObject largeHazard;
    //    largeHazard = ChooseLargeHazard();
    //    yield return new WaitForSeconds(WaitTime);
    //    Instantiate(largeHazard, PlaceToSpawn(), Quaternion.identity);
    //    Destroy(largeHazard);
    //}

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(WaitTime);
        TB.isOnGround = false;
    }
    
    IEnumerator Wait2()
    {
        
        yield return new WaitForSeconds(WaitTime);
        Instantiate(Thumbtack, PlaceToSpawn(), Quaternion.identity);
        TB.isOnGround = true;
    }

    private Vector3 PlaceToSpawn()
    {
        int number = Random.Range(0, spawnPoints.Count);
        return spawnPoints[number];
    }

    private GameObject ChooseLargeHazard()
    {
        int number = Random.Range(0, largeHazards.Count);
        return largeHazards[number];
    }


}
