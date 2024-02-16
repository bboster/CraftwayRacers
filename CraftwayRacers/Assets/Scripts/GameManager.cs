using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerInput PlayerInputInstance;
    public InputAction Pause;
    [SerializeField]
    private GameObject pauseScreen;
    // Start is called before the first frame update
    void Start()
    {
        PlayerInputInstance = GetComponent<PlayerInput>();
        PlayerInputInstance.currentActionMap.Enable();

        Pause = PlayerInputInstance.currentActionMap.FindAction("Pause");

        Pause.started += Pause_started;
        Pause.canceled += Pause_canceled;
    }

    private void Pause_canceled(InputAction.CallbackContext obj)
    {
        print("pause cancelled");
    }

    private void Pause_started(InputAction.CallbackContext obj)
    {
        Time.timeScale = 0.0f;
        pauseScreen.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("DavidScene");
        }
    }

    public void BackToGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1.0f;
    }

    private void OnDestroy()
    {
        Pause.started -= Pause_started;
        Pause.canceled -= Pause_canceled;
    }
}
