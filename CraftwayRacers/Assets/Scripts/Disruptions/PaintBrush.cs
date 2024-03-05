using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private GameObject Paint;
    public bool isWaiting = false;

    
    public IEnumerator SpawnPaintsDelay()
    {
        isWaiting = true;
        yield return new WaitForSeconds(1);
        Instantiate(Paint, Player.transform.position, Quaternion.identity);
        isWaiting = false;
    }
}
