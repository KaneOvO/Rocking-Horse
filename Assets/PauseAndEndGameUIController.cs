using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseAndEndGameUIController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject endGameCanvas;
    public bool IsPaused { get; private set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.pauseMenu = pauseMenu;
        GameManager.Instance.endGameCanvas = endGameCanvas;
        GameManager.Instance.PauseAndEndGameUIController = this;

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     TogglePause();
        // }
    }

    public void TogglePause()
    {
        if(pauseMenu == null)
            return;

        if (IsPaused)
            Resume();
        else
            Pause();
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScreenScene");
    }
}
