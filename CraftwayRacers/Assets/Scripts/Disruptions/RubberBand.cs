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
            rb = Speed.CarRb;
            AudioSource.PlayClipAtPoint(soundManager.GetComponent<SoundManager>().GetSound("BoostSound").clip, mainCam.transform.position);
            print("HERE");
                StartCoroutine(AddForce());
        }
    }

    IEnumerator AddForce()
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
    }
}
