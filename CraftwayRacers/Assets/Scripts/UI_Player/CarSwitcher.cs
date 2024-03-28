using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarSwitcher : MonoBehaviour
{
    int index = 0;
    [SerializeField] List<GameObject> cars = new List<GameObject>();
    PlayerInputManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<PlayerInputManager>();
        index = 0;
        manager.playerPrefab = cars[index];
    }

    public void SwitchNextSpawnCar(PlayerInput input)
    {
        index++;
        if (index >= cars.Count)
        {
            index = 0;
        }
        manager.playerPrefab = cars[index];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
