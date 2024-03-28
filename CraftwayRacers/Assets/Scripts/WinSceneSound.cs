using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinSceneSound : MonoBehaviour
{
    [SerializeField] private AudioClip winSound;

    private GameObject mainCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.Find("Main Camera");

        AudioSource.PlayClipAtPoint(winSound, mainCam.transform.position);
    }
}
