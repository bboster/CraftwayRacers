using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PaintBrush PB;
    public bool HasPaint;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (PB.isWaiting == false && HasPaint == true)
        {
            for(int i = 0; i < 4; i++)
            {
                StartCoroutine(PB.SpawnPaintsDelay());
            }
            HasPaint = false;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "GatchaBall Variant")
        {
            HasPaint = true;
            Destroy(collision.gameObject);
        }
    }
}
