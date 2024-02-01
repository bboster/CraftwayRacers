using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnPointManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private Vector3 spawn;
    [SerializeField]
    private int value;
    [SerializeField]
    private Vector3 where;
    [SerializeField]
    private Vector3 value1 = new Vector3(53.69f, 3.87f, -43.87f);
    [SerializeField]
    private Vector3 value2 = new Vector3(90.6f, 3.87f, -17.4f);
    [SerializeField]
    private Vector3 value3 = new Vector3(88.3f, 3.87f, 24.8f);
    [SerializeField]
    private Vector3 value4 = new Vector3(25.8f, 3.87f, 46.8f);
    [SerializeField]
    private Vector3 value5 = new Vector3(-17.2f, 3.87f, 46.8f);
    [SerializeField]
    private Vector3 value6 = new Vector3(-51.6f, 3.87f, 45.3f);
    [SerializeField]
    private Vector3 value7 = new Vector3(-83.8f, 3.87f, 1.7f);
    [SerializeField]
    private Vector3 value8 = new Vector3(-63.4f, 3.87f, -41.8f);

    private List<Vector3> spawnPoints = new List<Vector3>();

    [SerializeField]
    private int count = 8;

    private void Start()
    {
        spawnPoints.Add(value1);
        spawnPoints.Add(value2);
        spawnPoints.Add(value3);
        spawnPoints.Add(value4);
        spawnPoints.Add(value5);
        spawnPoints.Add(value6);
        spawnPoints.Add(value7);
        spawnPoints.Add(value8);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void SpawnEnemy()
    {
        if(count > 0)
        {
            value = Random.Range(0, spawnPoints.Count);
            where = spawnPoints[value];
            spawnPoints.RemoveAt(value);
            count--;
            Instantiate(enemy, where, Quaternion.identity);
        }
    }
}
