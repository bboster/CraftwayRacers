using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    // If the player collides with the gatcha ball, then they will spawn planes that will lower friction of cars
    // If hasPaintBrush == true, do not change friction.
    // If hasPaintBrush == false, change friction if collided with PLANE.
    // Have the PLANE only stay spawned for 6 seconds.
    public PlayerController PC;

    public void SpawnPaints()
    {

    }
}
