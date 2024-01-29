using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnPointManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private Vector3 spawn;
    [SerializeField]
    private Vector3 valueX;
    [SerializeField] 
    private float valueY;
    [SerializeField]
    private float valueZ;

    // Start is called before the first frame update
    void Start()
    {
       // valueX = new Vector3(Random.Range(-12f, 12f), 5.0f, Random.Range(-12f, 12f));
    }

    // Update is called once per frame
    void Update()
    {
        valueX = new Vector3(Random.Range(-12f, 12f), 5.0f, Random.Range(-12f, 12f));
    }

    public void SpawnEnemy()
    {
        Instantiate(enemy, valueX, Quaternion.identity);
    }
}
