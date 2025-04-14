using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Video;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject joinPanel;
    public GameObject tutorialPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;
    public GameObject countdownObject;           
    public VideoPlayer countdownVideoPlayer;
    public AudioSource menuMusic;
    private bool hasPlayedCountdown = false; 

    private void Start()
    {
        // Only show the main menu at the start
        mainMenuPanel.SetActive(true);
        joinPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void OpenJoin()
    {
        mainMenuPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        joinPanel.SetActive(true);
    }

    public void StartRace()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OpenTutorial()
    {
        joinPanel.SetActive(false);
        tutorialPanel.SetActive(true);

        if (!hasPlayedCountdown && countdownObject != null && countdownVideoPlayer != null)
        {
            countdownObject.SetActive(true);            
            countdownVideoPlayer.Play();                
            StartCoroutine(HandleCountdown());
            hasPlayedCountdown = true;                  
        }
    }
    private IEnumerator HandleCountdown()
    {
        // Mute music
        if (menuMusic != null)
            menuMusic.volume = 0f;

        // Show countdown and play video
        if (countdownObject != null)
            countdownObject.SetActive(true);

        if (countdownVideoPlayer != null)
            countdownVideoPlayer.Play();

        yield return new WaitForSeconds(6f);

        // Unmute music
        if (menuMusic != null)
            menuMusic.volume = 1f;

        // Destroy countdown
        if (countdownObject != null)
            Destroy(countdownObject);
    }



    public void BackToJoin()
    {
        joinPanel.SetActive(true);
        tutorialPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OpenCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }

    public void BackToMenu()
    {
        joinPanel.SetActive(false);
        settingsPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
