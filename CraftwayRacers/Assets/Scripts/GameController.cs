using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameController : NetworkBehaviour
{
    public NetworkVariable<Vector3> Player1Pos;
    public NetworkVariable<Vector3> Player2Pos;
    public NetworkVariable<Vector3> Player3Pos;
    public NetworkVariable<Vector3> Player4Pos;

    private void Start()
    {
        //Ensures that only one GameController is active in the scene at a time.
        if(GameObject.FindGameObjectsWithTag("GameController").Length > 1)
        {
            Destroy(gameObject);
        }

        StartCoroutine(PrintPos());
    }

    /// <summary>
    /// Prints the player positions variables every second for testing.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PrintPos()
    {
        for(; ; )
        {
            print(Player1Pos.Value);
            print(Player2Pos.Value);
            print(Player3Pos.Value);
            print(Player4Pos.Value);

            yield return new WaitForSeconds(1f);
        }
    }
}
