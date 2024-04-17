using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberBand : MonoBehaviour
{
    private Rigidbody rb;
    private ArcadeDriving2 Speed;
    public float BoostForce = 5f;
    public bool isBoosting = false;
    public float BoostDuration =2f;

    private GameObject mainCam;
    private GameObject soundManager;
    private GameObject BoostSymbol;
    private GameObject BoostVFX1;
    private GameObject BoostVFX2;
    private GameObject SmokeVFX1;
    private GameObject SmokeVFX2;
    private GameObject SpdLines;
    public Animator Animator;

    private void Awake()
    {
        mainCam = GameObject.Find("Main Camera");
        soundManager = GameObject.Find("SoundManager");
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Speed = collision.gameObject.GetComponent<ArcadeDriving2>();
            BoostSymbol = Speed.boostSymbol;
            BoostVFX1 = Speed.boostVFX1;
            BoostVFX2 = Speed.boostVFX2;
            SmokeVFX1 = Speed.smokeVFX1;
            SmokeVFX2 = Speed.smokeVFX2;
            SpdLines = Speed.spdLines;
            BoostSymbol.SetActive(true);
            BoostVFX1.SetActive(true);
            BoostVFX2.SetActive(true);
            SpdLines.SetActive(true);
            SmokeVFX1.SetActive(false);
            SmokeVFX2.SetActive(false);
            rb = Speed.CarRb;
            AudioSource.PlayClipAtPoint(soundManager.GetComponent<SoundManager>().GetSound("BoostSound").clip, mainCam.transform.position);
            //if(Animator.name == "Gatcha")
                //Animator.SetBool("ifOpen", true);
            print("HERE");
                StartCoroutine(AddForce(BoostVFX1, BoostVFX2, BoostSymbol, SmokeVFX1, SmokeVFX2, SpdLines));
        }
    }

    IEnumerator AddForce(GameObject b1, GameObject b2, GameObject bSym, GameObject s1, GameObject s2, GameObject sL)
    {
        isBoosting = true;

        float elapsedTime = 0f;

        while (elapsedTime < BoostDuration)
        {
            float currentSpeed = Vector3.Dot(rb.transform.forward, rb.velocity);
            float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(currentSpeed / Speed.TopSpeed));
            //Here we get the current speed of the car and lessen the effect of the boost the more speed you have. Balancing...
            rb.AddForce(rb.transform.forward * (BoostForce/(normalizedSpeed*5+0.1f)), ForceMode.Impulse);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        isBoosting = false;
        bSym.SetActive(false);
        b1.SetActive(false);
        b2.SetActive(false);
        sL.SetActive(false);
        s1.SetActive(true);
        s2.SetActive(true);
    }

    private void Opened()
    {
        Destroy(gameObject);
    }

}
