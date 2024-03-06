using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelInfo : MonoBehaviour
{
    public float StartXPosition;
    public float StartZPosition;

    private void Start()
    {
        StartXPosition= transform.localPosition.x;
        StartZPosition= transform.localPosition.z;
    }
}
