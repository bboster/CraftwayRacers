using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private Vector3 placeToSpawn;
    [SerializeField] private GameObject Paint;
    public bool isWaiting = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player = collision.gameObject;
            placeToSpawn = Player.transform.position;
            for (int i = 0; i < transform.childCount; i++)
            {
                if(transform.GetChild(i).GetComponent<Renderer>() != null)
                {
                    transform.GetChild(i).GetComponent<Renderer>().enabled = false;
                }
            }
            gameObject.GetComponent<Collider>().enabled = false;
            StartCoroutine(SpawnPaintsDelay());
        }
    }
    public IEnumerator SpawnPaintsDelay()
    {
        isWaiting = true;
        yield return new WaitForSeconds(0.5f);
        Instantiate(Paint, placeToSpawn, Quaternion.identity);
        isWaiting = false;
    }
}
