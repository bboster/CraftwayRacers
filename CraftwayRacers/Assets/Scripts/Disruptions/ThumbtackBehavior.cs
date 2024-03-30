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

            if (Speed.Shielded == true)
            {
                //Speed.Shielded = true;
                // Speed.Shield.SetActive(false);
                //StartCoroutine(IFrames());
                AudioSource.PlayClipAtPoint(soundManager.GetComponent<SoundManager>().GetSound("ItemBreak").clip, mainCam.transform.position);
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("NormalRoad"))
        {
            spawningSystem.IsWaiting = true;
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            rb.isKinematic = true;
        }


        /*IEnumerator IFrames()
        {
            yield return new WaitForSeconds(5);
            Car.Shielded = false;
        }*/
    }
}
