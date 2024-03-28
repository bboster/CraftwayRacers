using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CottonShield : MonoBehaviour
{
    private Material Ball;
    private GameObject mainCam;
    private GameObject soundManager;

    // Start is called before the first frame update
    void Start()
    {
        //Ball = GetComponentInChildren<MeshRenderer>().material; //Give the shield object a script and put this in it. After the player hits a jack, start blinking in and out
    }

    private void Awake()
    {
        mainCam = GameObject.Find("Main Camera");
        soundManager = GameObject.Find("SoundManager");
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(soundManager.GetComponent<SoundManager>().GetSound("ItemPickup").clip, mainCam.transform.position);
            Destroy(gameObject);
        }
    }
}
