using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCountdown : MonoBehaviour
{
    public GameObject Ready;
    public GameObject One;
    public GameObject Two;
    public GameObject Three;
    public GameObject GO;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(ReadySetGo());
    }

    IEnumerator ReadySetGo()
    {
        yield return new WaitForSeconds(2f);
        Ready.SetActive(true);

        yield return new WaitForSeconds(2f);
        Ready.SetActive(false);
        Three.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        Three.SetActive(false);
        Two.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        Two.SetActive(false);
        One.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        One.SetActive(false);
        GO.SetActive(true);

        yield return new WaitForSeconds(2f);
        GO.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ReadySetGo());
        }
    }
}
