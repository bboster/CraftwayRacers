using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbtackBehavior : MonoBehaviour
{
    public Rigidbody rb;
    public SpawningSystem spawningSystem;
    private ArcadeDriving2 Speed;

    private GameObject mainCam;
    private GameObject soundManager;

    public GameObject PoofVFX1;
    public GameObject PoofVFX2;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.Find("Main Camera");
        soundManager = GameObject.Find("SoundManager");

        rb = GetComponent<Rigidbody>();
        spawningSystem = GameObject.Find("SpawningSystemManager").GetComponent<SpawningSystem>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Speed = collision.gameObject.GetComponent<ArcadeDriving2>();
            AudioSource.PlayClipAtPoint(soundManager.GetComponent<SoundManager>().GetSound("CarCollisionSound").clip, mainCam.transform.position);
        }

        if (collision.gameObject.CompareTag("NormalRoad"))
        {
            spawningSystem.IsWaiting = true;
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            rb.isKinematic = true;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Speed = collision.gameObject.GetComponent<ArcadeDriving2>();
            if (Speed.Shielded == true)
            {
                //Speed.Shielded = true;
                // Speed.Shield.SetActive(false);
                //StartCoroutine(IFrames());
                AudioSource.PlayClipAtPoint(soundManager.GetComponent<SoundManager>().GetSound("ItemBreak").clip, mainCam.transform.position);
                PoofVFX1.SetActive(true);
                PoofVFX2.SetActive(true);
                ThumbtackDestroy();
            }
        }

        IEnumerator ThumbtackDestroy()
        {
            yield return new WaitForSeconds(0.8f);
            Destroy(gameObject);
        }
    }
}
